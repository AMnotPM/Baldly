﻿namespace Baldly.Models;

public class Url
{
    public int Id { get; set; }
    public string? OriginalLink { get; set; }
    public string? ShortLink { get; set; }
    public int NrOfClicks { get; set; }
    public string? UserId { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateUpdated { get; set; }

    public AppUser? User { get; set; }
}