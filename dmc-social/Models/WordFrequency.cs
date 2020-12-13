using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DmcSocial.Models
{
  public class WordFrequency : BaseEntity
  {
    public string Word { get; set; }
    public int PostId { get; set; }
    public long Frequency { get; set; }
  }
}