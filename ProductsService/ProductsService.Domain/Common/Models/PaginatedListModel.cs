﻿namespace ProductsService.Domain.Common.Models;

public class PaginatedListModel<T>
{
    public List<T> Items { get; set; } = new();
    public int CurrentPage { get; set; } = 1;
    public int TotalPages { get; set; } = 1;
}
