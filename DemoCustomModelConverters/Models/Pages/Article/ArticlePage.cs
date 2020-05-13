using System.ComponentModel.DataAnnotations;
using DemoCustomModelConverters.Models.Baseclasses;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace DemoCustomModelConverters.Models.Pages.Article
{
    [ContentType(DisplayName = "ArticlePage", GUID = "a6c76133-ed70-4a76-a0be-a17fb4b34237", Description = "")]
    public class ArticlePage : BasePage
    {
        [CultureSpecific]
        [Display(
            Name = "Title",
            Description = "",
            GroupName = SystemTabNames.Content,
            Order = 110)]
        public virtual string Title { get; set; }

        [Display(
            Name = "Main body",
            Description = "",
            GroupName = SystemTabNames.Content,
            Order = 120)]
        public virtual XhtmlString MainBody { get; set; }
    }
}