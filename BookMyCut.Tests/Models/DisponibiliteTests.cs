using BookMyCut.Data.Models;
using Xunit;

namespace BookMyCut.Tests.Models
{
    public class DisponibiliteTests
    {
        [Fact]
        public void Test_Disponibilite()
        {
            var dispo = new Disponibilite
            {
                CoiffeurId = 1,
                Debut = new DateTime(2026, 5, 11, 9, 0, 0),
                Fin = new DateTime(2026, 5, 11, 10, 0, 0)
            };

            Assert.Equal(1, dispo.CoiffeurId);
            Assert.Equal(new DateTime(2026, 5, 11, 9, 0, 0), dispo.Debut);
            Assert.Equal(new DateTime(2026, 5, 11, 10, 0, 0), dispo.Fin);
            Assert.False(dispo.EstReserve);
        }
    }
}
