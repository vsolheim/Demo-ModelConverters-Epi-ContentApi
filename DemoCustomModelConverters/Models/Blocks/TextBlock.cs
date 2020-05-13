using System.ComponentModel.DataAnnotations;
using DemoCustomModelConverters.Models.Baseclasses;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace DemoCustomModelConverters.Models.Blocks
{
    [ContentType(DisplayName = "TextBlock", GUID = "cc136d65-882b-44fd-81c1-3e1f0877244b", Description = "")]
    public class TextBlock : BaseBlock
    {

        [CultureSpecific]
        [Display(
            Name = "Title",
            Description = "",
            GroupName = SystemTabNames.Content,
            Order = 110)]
        public virtual string Title { get; set; }
    }
}