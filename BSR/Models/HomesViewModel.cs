namespace BSR.Models;

public class HomesViewModel
{
    public List<Home> Homes { get; set; }
    public int? MinPrice { get; set; }
    public int? MaxPrice { get; set; }
    public int? MinArea { get; set; }
    public int? MaxArea { get; set; }
    public int? MinGarage { get; set; }
    public int? MinBedrooms { get; set; }
    public int? MinBathrooms { get; set; }
    public List<string> States { get; set; }
    public List<string> Cities { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public PaginationInfo PaginationInfo { get; set; }
}

public class PaginationInfo
{
    public int CurrentPage { get; set; }
    public int TotalItems { get; set; }
    public int ItemsPerPage { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalItems / ItemsPerPage);
    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;
}