using MyBGList.ValidationAttributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyBGList.DTO;

public class RequestDTO
{
    [DefaultValue(0)]
    public int PageIndex { get; set; } = 0;

    [DefaultValue(10), Range(0, 100)]
    public int PageSize { get; set; } = 10;

    [DefaultValue("Name"), NameOfProperty(typeof(BoardGameDTO))]
    public string? SortColumn { get; set; } = "Name";

    [DefaultValue("ASC"), AllowedStrings(["ASC", "DESC"])]
    public string? SortOrder { get; set; } = "ASC";

    [DefaultValue(null)]
    public string? FilterQuery { get; set; } = null;
}
