﻿namespace OrderService.Application.Exceptions;

public class CartException : Exception
{
	public CartException(string message) : base(message) { }
}