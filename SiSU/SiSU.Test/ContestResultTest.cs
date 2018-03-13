using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SiSU.Services;

namespace SiSU.Test
{
    [TestClass]
    public class ContestResultTest
    {
        Mock<ILeaderboardService> _mockContestService = new Mock<ILeaderboardService>();

        [TestInitialize]
        public void Init()
        {

        }

        [TestMethod]
        public void MatchDrawn()
        {
            //Arrange
            var matchResult = new MatchResult(1);
            var participantOne = new Participant(_mockContestService.Object) { CompetitorId = 10, ContestId = 1 };
            var participantTwo = new Participant(_mockContestService.Object) { CompetitorId = 20, ContestId = 1 };
            matchResult.AddParticipant(participantOne);
            matchResult.AddParticipant(participantTwo);
            matchResult.IsDrawn = true;

            //Act
            matchResult.Notify();

            //Assert
            _mockContestService.Verify(ls => ls.Add(1, 10, Infrastructure.GameResult.Drawn), Times.AtLeastOnce);
            _mockContestService.Verify(ls => ls.Add(1, 20, Infrastructure.GameResult.Drawn), Times.AtLeastOnce);

        }

        [TestMethod]
        public void MatchNotDrawnParticipantOneWon()
        {
            //Arrange
            var matchResult = new MatchResult(1);
            var participantOne = new Participant(_mockContestService.Object) { CompetitorId = 10, ContestId = 1 };
            var participantTwo = new Participant(_mockContestService.Object) { CompetitorId = 20, ContestId = 1 };
            matchResult.AddParticipant(participantOne);
            matchResult.AddParticipant(participantTwo);
            matchResult.IsDrawn = false;
            matchResult.WinnerId = participantOne.CompetitorId;

            //Act
            matchResult.Notify();

            //Assert
            _mockContestService.Verify(ls => ls.Add(1, 10, Infrastructure.GameResult.Won), Times.AtLeastOnce);
            _mockContestService.Verify(ls => ls.Add(1, 20, Infrastructure.GameResult.Lost), Times.AtLeastOnce);

        }

        [TestMethod]
        public void MatchNotDrawnParticipantTwoWon()
        {
            //Arrange
            var matchResult = new MatchResult(1);
            var participantOne = new Participant(_mockContestService.Object) { CompetitorId = 10, ContestId = 1 };
            var participantTwo = new Participant(_mockContestService.Object) { CompetitorId = 20, ContestId = 1 };
            matchResult.AddParticipant(participantOne);
            matchResult.AddParticipant(participantTwo);
            matchResult.IsDrawn = false;
            matchResult.WinnerId = participantTwo.CompetitorId;

            //Act
            matchResult.Notify();

            //Assert
            _mockContestService.Verify(ls => ls.Add(1, 10, Infrastructure.GameResult.Lost), Times.AtLeastOnce);
            _mockContestService.Verify(ls => ls.Add(1, 20, Infrastructure.GameResult.Won), Times.AtLeastOnce);

        }

    }
}
