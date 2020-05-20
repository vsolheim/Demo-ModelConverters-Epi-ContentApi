using System.ComponentModel.DataAnnotations;
using DemoCustomModelConverters.Models.Baseclasses;
using DemoCustomModelConverters.Models.Blocks;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace DemoCustomModelConverters.Models.Pages.Start
{
    [ContentType(DisplayName = "StartPage", GUID = "727a8a65-4436-4750-afe1-c2e5d5f50312", Description = "")]
    public class StartPage : BasePage
    {
        [Display(
            Name = "Title",
            Description = "",
            GroupName = SystemTabNames.Content,
            Order = 100)]
        public virtual string Title { get; set; }

        [Display(
            Name = "Blocks",
            Description = "",
            GroupName = SystemTabNames.Content,
            Order = 110)]
        public virtual ContentArea Blocks { get; set; }


        [Display(
            Name = "Main body",
            Description = "",
            GroupName = SystemTabNames.Content,
            Order = 120)]
        public virtual XhtmlString MainBody { get; set; }

        [Display(
            Name = "Text block",
            Description = "",
            GroupName = SystemTabNames.Content,
            Order = 130)]
        public virtual TextBlock TextBlock { get; set; }
    }
}