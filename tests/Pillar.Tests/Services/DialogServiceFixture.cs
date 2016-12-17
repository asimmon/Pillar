using Moq;
using Pillar.Interfaces;
using Pillar.Services;
using Xunit;

namespace Pillar.Tests.Services
{
    public class DialogServiceFixture
    {
        [Fact]
        public void DisplaysAlert()
        {
            var page = new Mock<IPage>();
            var dialogService = new DialogService(page.Object);

            dialogService.DisplayAlert("Alert", "You have been alerted", "OK");

            page.Verify(x => x.DisplayAlert("Alert", "You have been alerted", "OK"));
        }

        [Fact]
        public async void DisplaysAlertWithResponse()
        {
            var page = new Mock<IPage>();
            page.Setup(x => x.DisplayAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(true);

            var dialogService = new DialogService(page.Object);

            var answer = await dialogService.DisplayAlert("Question?", "Would you like to play a game", "Yes", "No");

            page.Verify(x => x.DisplayAlert("Question?", "Would you like to play a game", "Yes", "No"));
            Assert.True(answer);
        }

        [Fact]
        public async void DisplaysActionSheetWithResponse()
        {
            var page = new Mock<IPage>();
            page.Setup(x => x.DisplayActionSheet("ActionSheet: Send to?", "Cancel", null, "Email", "Twitter", "Facebook"))
                .ReturnsAsync("Yes");

            var dialogService = new DialogService(page.Object);

            var answer = await dialogService.DisplayActionSheet("ActionSheet: Send to?", "Cancel", null, "Email", "Twitter", "Facebook");

            page.Verify(x => x.DisplayActionSheet("ActionSheet: Send to?", "Cancel", null, "Email", "Twitter", "Facebook"));
            Assert.Equal("Yes", answer);
        }

    }
}
