﻿namespace ProductsService.Application.DTO;

public class AttributeValueDTO
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string Value { get; set; }
    public int AttributeId {  get; set; } 
}
