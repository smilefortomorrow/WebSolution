﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WorkFlowSystem.Entities;

[Table("Invoice")]
public partial class Invoice
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int ID { get; set; }
    [StringLength(15)][Unicode(false)] public string InvoiceNo { get; set; }
    [Required] public int ClientID { get; set; }
    [Required][StringLength(20)][Unicode(false)] public string Username { get; set; }
    [Required][StringLength(30)][Unicode(false)] public string Email { get; set; }
    [Required] public int PackageCount { get; set; }
    [Required] public double Total { get; set; }
    [Required][StringLength(10)][Unicode(false)] public string Status { get; set; }
    [Required][Column(TypeName = "date")] public DateTime Regist { get; set; } // 'date' SQL type is mapped to 'DateTime' in .NET
    [Required] public double GST { get; set; } // Nullable int, since 'Allow Nulls' is checked
    [StringLength(20)][Unicode(false)] public string City { get; set; }
    [StringLength(20)][Unicode(false)] public string Country { get; set; }
    [StringLength(10)][Unicode(false)] public string PostalCode { get; set; }
    [StringLength(20)][Unicode(false)] public string BusinessNo { get; set; }
}