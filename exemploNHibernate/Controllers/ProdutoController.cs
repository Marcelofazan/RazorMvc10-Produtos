using exemploNHibernate.Models;
using exemploNHibernate.Repositorio;
using Microsoft.AspNetCore.Mvc;

namespace exemploNHibernate.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly ProdutoRepositorio _produtoRepository;

        public ProdutoController(NHibernate.ISession session) => _produtoRepository = new ProdutoRepositorio(session);

        public ActionResult Index()
        {
            return View(_produtoRepository.FindAll().ToList());
        }

        public async Task<ActionResult> Details(long? id)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            Produto produto = await _produtoRepository.FindByID(id.Value);
            if (produto == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return View(produto);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Nome,Quantidade,Preco")] Produto produto)
        {
            if (ModelState.IsValid)
            {
                await _produtoRepository.Add(produto);
                return RedirectToAction("Index");
            }

            return View(produto);
        }

        public async Task<ActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            Produto produto = await _produtoRepository.FindByID(id.Value);
            if (produto == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return View(produto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind("Id,Nome,Quantidade,Preco")] Produto produto)
        {
            if (ModelState.IsValid)
            {
                await _produtoRepository.Update(produto);
                return RedirectToAction("Index");
            }
            return View(produto);
        }

        public async Task<ActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return StatusCode(StatusCodes.Status400BadRequest);
            }
            Produto produto = await _produtoRepository.FindByID(id.Value);
            if (produto == null)
            {
                return StatusCode(StatusCodes.Status404NotFound);
            }
            return View(produto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(long id)
        {
            await _produtoRepository.Remove(id);
            return RedirectToAction("Index");
        }
    }
}
