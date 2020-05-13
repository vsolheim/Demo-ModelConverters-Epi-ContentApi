using System.ComponentModel.DataAnnotations;
using DemoCustomModelConverters.Models.Baseclasses;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace DemoCustomModelConverters.Models.Pages.Start
{
    [ContentType(DisplayName = "StartPage", GUID = "727a8a65-4436-4750-afe1-c2e5d5f50312", Description = "")]
    public class StartPage : BasePage
    {
        [CultureSpecific]
        [Display(
            Name = "Title",
            Description = "",
            GroupName = SystemTabNames.Content,
            Order = 100)]
        public virtual string Title { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Blocks",
            Description = "",
            GroupName = SystemTabNames.Content,
            Order = 110)]
        public virtual ContentArea Blocks { get; set; }
    }
}