// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using TP1_Web4.Data;
using TP1_Web4.Models;

namespace TP1_Web4.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<Utilisateur> _signInManager;
        private readonly UserManager<Utilisateur> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initialise le modèle de la page de création d'un compte avec les services requis.
        /// </summary>
        /// <param name="userManager">Le gestionnaire des utilisateurs</param>
        /// <param name="signInManager">Le gestionnaire de session</param>
        /// <param name="logger">Le gestionnaire de journalisation</param>
        /// <param name="context">Le contexte de la base de données</param>
        public RegisterModel(
            UserManager<Utilisateur> userManager,
            SignInManager<Utilisateur> signInManager,
            ILogger<RegisterModel> logger,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {

            [Required (ErrorMessage = "Le nom est requis")]
            public string Nom { get; set; }

            [Required(ErrorMessage = "Le prénom est requis")]
            public string Prenom { get; set; }

            [Required(ErrorMessage = "Le nom d'utilisateur est requis")]
            public string UserName { get; set; }

            [RegularExpression(@"^[A-Z]\d[A-Z]\d[A-Z]\d$", ErrorMessage = "Le texte entré ne correspond pas à un code postal.")]
            [Required(ErrorMessage = "Vous devez entrer un code postal")]
            public string CodePostal { get; set; }


            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Courriel")]
            public string Email { get; set; }

            [Required]
            public string Addresse { get; set; }

            [Required]
            public string Ville { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "Le {0} doit être entre {2} et {1} caractères de long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Mot de passe")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirmer le mot de passe")]
            [Compare("Password", ErrorMessage = "Le mot de passe et sa confirmation ne correspondent pas.")]
            public string ConfirmPassword { get; set; }
        }

        public List<Utilisateur> Utilisateurs { get; set; }

        /// <summary>
        /// Charge la page web de création d'un compte utilisateur.
        /// </summary>
        /// <param name="returnUrl">L'url à laquelle retourner après la connexion de l'utilisateur.</param>
        /// <returns>Cette fonction sert à charger la page web en asynchrone et retourne quand elle finit cette tâche.</returns>
        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        /// <summary>
        /// Valide les informations du nouveau compte utilisateur, le créée, se connecte à ce compte si la validation n'échoue pas, et
        /// optionnellement redirige vers une url spécifique après la création du compte utilisateur.
        /// </summary>
        /// <param name="returnUrl">L'url à laquelle retourner après la création de l'utilisateur.</param>
        /// <returns>Cette fonction sert à charger la page web en asynchrone et retourne quand elle finit cette tâche.</returns>
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            if (ModelState.IsValid)
            {
                Utilisateurs = _context.Users.Where(u => u.Email == Input.Email).ToList();
                if (Utilisateurs.Count > 0)
                {
                    ModelState.AddModelError("Input.Email", "Un utilisateur avec le même courriel existe déjà.");
                    return Page();
                }
                
                var user = new Utilisateur() { UserName = Input.UserName, Email = Input.Email,
                    Nom = Input.Nom, Prenom = Input.Prenom, CodePostal = Input.CodePostal,
                    Addresse = Input.Addresse, Ville = Input.Ville };
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Utilisateur");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
