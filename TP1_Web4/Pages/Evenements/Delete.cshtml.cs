using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TP1_Web4.Data;
using TP1_Web4.Models;

namespace TP1_Web4.Pages.Evenements
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly TP1_Web4.Data.ApplicationDbContext _context;

        /// <summary>
        ///  Initialise la page web Delete avec les services nécessaires.
        /// </summary>
        /// <param name="context">Le contexte de la base de données</param>
        public DeleteModel(TP1_Web4.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Evenement Evenement { get; set; } = default!;

        /// <summary>
        /// Charge le formulaire de confirmation de suppression d'un évènement.
        /// </summary>
        /// <param name="id">L'id de l'évènement à archiver.</param>
        /// <returns>Cette fonction sert à charger la page web en asynchrone et retourne quand elle finit cette tâche.</returns>
        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evenement = await _context.Evenements.FirstOrDefaultAsync(m => m.Id == id);

            if (evenement is not null)
            {
                if (Evenement.Auteur.UserName != User.Identity.Name)
                    return Forbid();
                Evenement = evenement;

                return Page();
            }

            return NotFound();
        }

        /// <summary>
        /// Archive l'évènement dans la base de données, et redirige vers la page des évènements de l'utilisateur.
        /// </summary>
        /// <param name="id">L'id de l'évènement à archiver.</param>
        /// <returns>Cette fonction sert à charger la page web en asynchrone et retourne quand elle finit cette tâche.</returns>
        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var evenement = await _context.Evenements.FindAsync(id);
            if (evenement != null)
            {
                Evenement = evenement;
                Evenement.Annule = true;
                _context.Attach(Evenement).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            TempData["Confirmation"] = "L'évènement a bien été annulé.";
            return RedirectToPage("./Index");
        }
    }
}
