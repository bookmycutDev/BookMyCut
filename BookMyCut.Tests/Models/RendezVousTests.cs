using BookMyCut.Data.Models;
using Xunit;

namespace BookMyCut.Tests.Models
{
    public class RendezVousTests
    {
        [Fact]
        public void Test_RendezVous()
        {
            var rdv = new RendezVous
            {
                ClientId = 1,
                CoiffeurId = 2,
                ServiceId = 3,
                DateHeure = new DateTime(2026, 5, 11, 14, 0, 0)
            };

            Assert.Equal(1, rdv.ClientId);
            Assert.Equal(2, rdv.CoiffeurId);
            Assert.Equal(3, rdv.ServiceId);
            Assert.Equal(new DateTime(2026, 5, 11, 14, 0, 0), rdv.DateHeure);
            Assert.Equal("Confirmé", rdv.Statut);
        }
    }
}