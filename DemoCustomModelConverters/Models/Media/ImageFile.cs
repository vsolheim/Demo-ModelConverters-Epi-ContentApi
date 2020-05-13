using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Framework.DataAnnotations;

namespace DemoCustomModelConverters.Models.Media
{
    /// <summary>
    /// Filetype for image filetypes.
    /// </summary>
    [ContentType(
        DisplayName = "ImageFile",
        GUID = "b8d73bd2-2938-4d57-ae8b-3b4e2cdc3f7c",
        Description = "")]
    [MediaDescriptor(ExtensionString = "jpg,jpeg,jpe,ico,gif,bmp,png")]
    public class ImageFile : ImageData
    {
        [Display(Order = 100, GroupName = SystemTabNames.Content, Name = "Image text")]
        public virtual string ImageText { get; set; }
    }
}