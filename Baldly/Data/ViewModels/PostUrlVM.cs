using System.ComponentModel.DataAnnotations;

namespace Baldly.Data.ViewModels;

public class PostUrlVm
{
    [Required(ErrorMessage = "Url is required")]
    [RegularExpression(@"^http(s)?://([\w-]+.)+[\w-]+(/[\w- ./?%&=]*)?$",
        ErrorMessage = "The value is not a valid URL")]
    public string? Url { get; set; }
}

// RegularExpression("^http(s)?://([\\w-]+.)+[\\w-]+(/[\\w- ./?%&=])?$",