﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WorkFlowSystem.Entities;

[Table("BaseMetal")]
public partial class BaseMetal
{
    [Key]
    public int BaseMetalID { get; set; }

    public int? WPSFormID { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string PNumber { get; set; }

    public int? GroupNumber { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string UNSNumber { get; set; }

    public bool? ChemicalAnalysisStated { get; set; }

    public double? ThicknessRangeMax { get; set; }

    public bool? MaxPassThicknessGreaterThan13mm { get; set; }

    [ForeignKey("WPSFormID")]
    [InverseProperty("BaseMetals")]
    public virtual WP WPSForm { get; set; }
}