using System;
using Avalonia.Controls;
using MsBox.Avalonia;
using MsBox.Avalonia.Dto;
using MsBox.Avalonia.Enums;
using MsBox.Avalonia.Models;

namespace MarketProject.Models.Exceptions;

public class MaxMinException : Exception
{
    public MaxMinException()
    { }

    public MaxMinException(string message) : base(message)
    { }

    public MaxMinException(string message, Exception inner) : base(message, inner)
    { }
}