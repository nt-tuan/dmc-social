using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DmcSocial.Models;

namespace DmcSocial.Repositories
{
  public static class Helper
  {
    readonly static string ACCEPT_TAG_PATTER = @"[^\p{L}\p{N}]";
    public static IQueryable<T> ApplyPaging<T>(IQueryable<T> query, GetListParams<T> pagingParam)
    {
      var chain = query;
      chain = chain.Skip(pagingParam.Offset).Take(pagingParam.Limit);
      if (pagingParam.OrderBy != null)
      {
        if (pagingParam.OrderDirection == GetListParams<T>.OrderDirections.ASC)
        {
          chain = chain.OrderBy(pagingParam.OrderBy);
        }
        else
        {
          chain = chain.OrderByDescending(pagingParam.OrderBy);
        }
      }
      return chain;
    }

    private static string ConvertToUnSign3(string s)
    {
      Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
      string temp = s.Normalize(NormalizationForm.FormD);
      return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D');
    }
    public static string NormalizeString(string str)
    {
      if (string.IsNullOrEmpty(str))
        return str;
      var normalized = str;
      normalized = normalized.ToLower();
      normalized = ConvertToUnSign3(normalized);
      normalized = Regex.Replace(normalized, ACCEPT_TAG_PATTER, "");
      return normalized;
    }
  }
}