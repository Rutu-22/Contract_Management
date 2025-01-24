
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ContactsApi.Models;

public class Contact
{
    public int Id { get; set; }

    [Required(ErrorMessage = "FirstName is required.")]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "LastName is required.")]
    public string? LastName { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid Email format.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Status is required.")]
    public string? Status { get; set; } // Status can be "Approved" or "Rejected"

    public string? Comment { get; set; }
}


public class ListResponseModel<T>
{
    public List<T> data { get; set; }
    public string msg { get; set; }
    public string code { get; set; }
}
public class ResponseModel<T>
{
    public T data { get; set; }
    public string msg { get; set; }
    public string code { get; set; }
}
