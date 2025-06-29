using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Nominas.Models
{
	public class ApplicationUser : IdentityUser
	{
		[Required]
		public int Nombre { get; set; }
	}
}
