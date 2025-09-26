using Xunit;
using Moq;
using MyApp.Services; // pas aan naar jouw namespace
using MyApp.Models;

public class RegistratieServiceTests
{
    [Fact]
    public void Registreer_MetGeldigeGegevens_GeeftSucces()
    {
        // Arrange
        var gebruiker = new Gebruiker
        {
            Gebruikersnaam = "TestUser",
            Email = "test@example.com",
            Wachtwoord = "SterkW8woord!"
        };

        var mockDb = new Mock<IDatabaseService>();
        var mockMail = new Mock<IEmailService>();

        mockDb.Setup(db => db.SlaGebruikerOp(It.IsAny<Gebruiker>()))
              .Returns(true);
        mockMail.Setup(m => m.StuurBevestiging(It.IsAny<string>()))
                .Returns(true);

        var registratieService = new RegistratieService(mockDb.Object, mockMail.Object);

        // Act
        var resultaat = registratieService.Registreer(gebruiker);

        // Assert
        Assert.True(resultaat.Succes);
        mockDb.Verify(db => db.SlaGebruikerOp(It.IsAny<Gebruiker>()), Times.Once);
        mockMail.Verify(m => m.StuurBevestiging(gebruiker.Email), Times.Once);
    }
}
