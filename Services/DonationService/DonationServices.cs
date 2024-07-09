using Ikagai.Core;
using Ikagai.Dtos;
using Ikagai.Email;
using Ikagai.Services.EmailService;
using Microsoft.EntityFrameworkCore;

namespace Ikagai.Services.DonationService
{
    public class DonationServices : IDonationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailServices _emailServices;

        public DonationServices(ApplicationDbContext context, IEmailServices emailServices)
            => (_context, _emailServices) = (context, emailServices);

        public async Task<DonationResponce> CreateDonationRequestAsync(DonationRequestDto dto)
        {
            var request = await MapRequestDtoToRequest(dto, (await _context.People.FindAsync(dto.PersonId)).Gender);
            await _context.AddAsync(request);
            await _context.SaveChangesAsync();
            return MapDonationToResponce(request);
        }

        public async Task<List<BloodBanksDonation>> GetBloodBanks(Guid requestId)
        {

            var request = await _context.DonationRequests.FindAsync(requestId);

            var bloodBanks = await _context.BloodBanks
                .Where(b => b.GovernorateId == request.GovernorateId).ToListAsync();

            var bloodBankDonation = new List<BloodBanksDonation>();

            foreach (var bloodBank in bloodBanks)
            {
                bloodBankDonation.Add(
                    new BloodBanksDonation
                    {
                        BloodBankId = bloodBank.Id,
                        BloodBankName = bloodBank.BloodBankName,
                        Governorate = await GetGovernorate(bloodBank.GovernorateId),
                        City = await GetCity(bloodBank.CityId),
                        Location = bloodBank.Location,
                        ClosedHour = bloodBank.ClosedHour,
                        OpenHour = bloodBank.OpenHour
                    });
            }

            return bloodBankDonation;
        }

        //Donor Choose a BloodBank
        public async Task ChooseBloodBank(Guid requestId, Guid bloodBankId)
        {
            var request = await _context.DonationRequests.FindAsync(requestId);
            request.BloodBankId = bloodBankId;
            await _context.SaveChangesAsync();
            await SendMessage(bloodBankId);
        }


        //Get Donors Foreach bloodBank 
        public async Task<List<BloodBankDonor>> GetBloodBankDonors(Guid bloodBankId)
        {
            var bloodBank = await _context.BloodBanks.FindAsync(bloodBankId);
            var bloodBankRequests = await _context.DonationRequests.Where(r => r.BloodBankId == bloodBankId).ToListAsync();
            var donors = new List<BloodBankDonor>();
            foreach (var request in bloodBankRequests)
            {
                var person = await _context.People.FindAsync(request.PersonId);
                var bloodType = await _context.BloodAndDerivatives.FindAsync(person.BloodAndDerivativesId);
                var user = await _context.Users.FindAsync(person.ApplicationUserId);

                donors.Add(new BloodBankDonor
                {
                    DonationRquestId = request.Id,
                    DonorId = person.Id,
                    Donor = person.FirstName + " " + person.LastName,
                    BloodType = bloodType.Name,
                    Gender = person.Gender ? "Male" : "Female",
                    PhoneNumber = user.PhoneNumber,
                    RequestDate = request.RequestDate
                });
            }
            return donors;
        }

        private async Task SendMessage(Guid bloodBankId)
            => await SendEmail((await _context.Users.FindAsync((await _context.BloodBanks.FindAsync(bloodBankId)).ApplicationUserId)).Email,
                $"SomeOne Want To Donate Go To 'Donors' Page and Accept him!");


        public async Task<string> GetGovernorate(byte Id)
            => (await _context.Governorates.FindAsync(Id)).Name;

        public async Task<string> GetCity(byte Id)
            => (await _context.Cities.FindAsync(Id)).Name;

        private async Task<DonationRequest> MapRequestDtoToRequest(DonationRequestDto dto, bool gender)
        {
            var status = GetStatusd(dto.StatusId);

            return new DonationRequest()
            {
                CityId = dto.CityId,
                GovernorateId = dto.GovernorateId,
                LastDonationDate = DateOnly.FromDateTime(dto.LastDonationDate),
                Location = dto.Location,
                StatusId = status == 4 ? (byte)4 : status == 8 ? (byte)8 : (validLastDonationDate(DateOnly.FromDateTime(dto.LastDonationDate), gender)) ? (byte)7 : (byte)8,
                PersonId = dto.PersonId,
            };
        }
        private DonationResponce MapDonationToResponce(DonationRequest request)
            => new DonationResponce { DonationRequestId = request.Id, RequestStatus = request.StatusId };

        private bool validLastDonationDate(DateOnly LastDonationDate, bool gender)
        {
            DateTime currentDate = DateTime.Today;
            int monthsDifference = ((currentDate.Year - LastDonationDate.Year) * 12) + (currentDate.Month - LastDonationDate.Month);
            return gender ? monthsDifference > 3 || (monthsDifference == 3 && currentDate.Day >= LastDonationDate.Day)
                          : monthsDifference > 4 || (monthsDifference == 4 && currentDate.Day >= LastDonationDate.Day);
        }

        public async Task SendEmail(string email, string content)
            => await _emailServices.SendEmailAsync(new Message(new string[] { email }, "Blood Donation", content));

        public byte GetStatusd(List<byte> Ids)
          => Ids.Contains(4) ? (byte)4 : Ids.Contains(8) ? (byte)8 : (byte)7;
    }
}
