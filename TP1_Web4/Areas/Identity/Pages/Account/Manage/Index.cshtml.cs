// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TP1_Web4.Models;

namespace TP1_Web4.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<Utilisateur> _userManager;
        private readonly SignInManager<Utilisateur> _signInManager;
        private readonly TP1_Web4.Data.ApplicationDbContext _context;

        /// <summary>
        /// Initialise la page de modification des informations générales de l'utilisateur avec les services requis.
        /// </summary>
        /// <param name="userManager">le gestionnaire des utilisateurs.</param>
        /// <param name="signInManager">Le gestionnaire de session.</param>
        /// <param name="context">Le contexte de la base de données.</param>
        public IndexModel(
            UserManager<Utilisateur> userManager,
            SignInManager<Utilisateur> signInManager,
            TP1_Web4.Data.ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        /// 
        [BindProperty]
        [DisplayName("Nom d'utilisateur")]
        public string Username { get; set; }

        [BindProperty]
        public string Nom { get; set; }

        [BindProperty]
        public string Prenom { get; set; }

        [BindProperty]
        public string CodePostal { get; set; }

        [BindProperty]
        public string Addresse { get; set; }

        [BindProperty]
        public string Ville { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

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
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        /// <summary>
        /// Charge les informations courantes de l'utilisateur dans les champs de modification des données.
        /// </summary>
        /// <param name="user">L'utilisateur à modifier</param>
        /// <returns>Cette fonction sert à charger la page web en asynchrone et retourne quand elle finit cette tâche.</returns>
        private async Task LoadAsync(Utilisateur user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;
            Nom = user.Nom;
            Prenom = user.Prenom;
            Addresse = user.Addresse;
            Ville = user.Ville;
            CodePostal = user.CodePostal;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber
            };
        }

        /// <summary>
        /// Initialise la page de modification des informations générales de l'utilisateur.
        /// </summary>
        /// <returns>Cette fonction sert à charger la page web en asynchrone et retourne quand elle finit cette tâche.</returns>
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
           
            await LoadAsync(user);
            return Page();
        }

        /// <summary>
        /// Valide les informations entrés par l'utilisateur, modifie dans la base de données les informations qui diffèrent de ce qu'elles
        /// étaient auparavant et qui sont valides, et informe l'utilisateur de la modification de ses informations de profil.
        /// </summary>
        /// <returns>Cette fonction sert à charger la page web en asynchrone et retourne quand elle finit cette tâche.</returns>
        public async Task<IActionResult> OnPostAsync()
        {
            Utilisateur user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            if (!(string.IsNullOrEmpty(Nom)) && Nom != user.Nom)
                user.Nom = Nom;

            if (!(string.IsNullOrEmpty(Prenom)) && Prenom != user.Prenom)
                user.Prenom = Prenom;

            if (!(string.IsNullOrEmpty(Ville)) && Ville != user.Ville)
                user.Ville = Ville;

            if (!(string.IsNullOrEmpty(Addresse)) && Addresse != user.Addresse)
                user.Addresse = Addresse;

            if (!(string.IsNullOrEmpty(CodePostal)) && (Regex.IsMatch(CodePostal, @"^(?:[ABCEGHJKLMNPRSTVXY]\d){3}$")) && CodePostal != user.CodePostal)
                user.CodePostal = CodePostal;

            _context.Attach(user).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Une erreur inconnue est survenue lors du changement du numéro de téléphone.";
                    return RedirectToPage();
                }
            }
            var userName = await _userManager.GetUserNameAsync(user);
            if (Username != userName)
            {
                var setResult = await _userManager.SetUserNameAsync(user, Username);
                if (!setResult.Succeeded)
                {
                    StatusMessage = "Une erreur inconnue est survenue lors du changement du nom d'utilisateur.";
                    return RedirectToPage();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
