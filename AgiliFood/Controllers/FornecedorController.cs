using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgiliFood.Models;

namespace AgiliFood.Controllers
{
    public class FornecedorController : Controller
    {
        //Busca no banco todos os dados da tabela "tb_fornecedor" e retorna na View "ConsultarFornecedor"
        [HttpGet]
        public ActionResult ConsultarFornecedor()
        {
            try
            {
                banco objBanco = new banco();
                List<tb_fornecedor> lstFornecedor = objBanco.tb_fornecedor.Where(p => p.id_rh == UtilController.idRh).ToList();

                if (lstFornecedor.Count == 0)
                {
                    ModelState.AddModelError("Error", "Não possui fornecedores cadastrados.");
                    return View(lstFornecedor);

                }
                else
                {
                    return View(lstFornecedor);
                }
            }
            catch
            {
                return HttpNotFound("Ocorreu um erro na operação.");

            }
        }

        //Retorna a View "InserirFornecedor"
        [HttpGet]
        public ActionResult InserirFornecedor()
        {
            return View();
        }

        //Método criado para gravar os dados da View "InserirFornecedor" no banco de dados na tabela "tb_fornecedor"
        [HttpPost]
        public ActionResult InserirFornecedor(tb_fornecedor objFornecedor)
        {
            if(objFornecedor.nome_fornecedor == null)
            {
                ModelState.AddModelError("Error", "Preecher todo(s) o(s) campo(s). *Exceto o campo ATIVO*");
                return View();
            }

            else
            {
                try
                {
                    objFornecedor.id_rh = UtilController.idRh;
                    banco objBanco = new banco();
                    objBanco.tb_fornecedor.Add(objFornecedor);
                    objBanco.SaveChanges();

                    ModelState.Clear();
                    ModelState.AddModelError("Success", "Dados gravados com sucesso.");
                    return View();
                }

                catch
                {
                    ModelState.AddModelError("Error", "Ocorreu um erro na operação.");
                    return View();
                }
            }
        }

        //Recebe o idFornecedor da View "ConsultarFornecedor", faz uma busca no banco, e retorna todos os dados referentes ao idFornecedor na View "EditarFornecedor"
        [HttpGet]
        public ActionResult EditarFornecedor(int idFornecedor)
        {
            banco objBanco = new banco();
            tb_fornecedor objResg = objBanco.tb_fornecedor.Find(idFornecedor);
            return View(objResg);
        }

        //Método criado para gravar os dados da View "EditarFornecedor" no banco de dados na tabela "tb_fornecedor" referente ao mesmo idFornecedor recebido
        //da View "ConsultarFornecedor"
        [HttpPost]
        public ActionResult EditarFornecedor(tb_fornecedor objFornecedor)
        {
            try
            {
                banco objBanco = new banco();
                tb_fornecedor objResg = objBanco.tb_fornecedor.FirstOrDefault(p => p.id_fornecedor == objFornecedor.id_fornecedor);

                objResg.nome_fornecedor = objFornecedor.nome_fornecedor;
                objResg.status_fornecedor = objFornecedor.status_fornecedor;

                objBanco.SaveChanges();
                ModelState.AddModelError("Success", "Dados alterados com sucesso.");
                return View();
            }
            catch
            {
                ModelState.AddModelError("Error", "Ocorreu um erro na operação.");
                return View();
            }
          
        }

        //Retorna a View "DeletarFornecedor" com os dados referente ao idFornecedor recebido da View "ConsultarFornecedor"
        [HttpGet]
        public ActionResult DeletarFornecedor(int idFornecedor)
        {
            UtilController.idFornecedor = idFornecedor;
            banco objBanco = new banco();
            tb_fornecedor objResg = objBanco.tb_fornecedor.FirstOrDefault(p => p.id_fornecedor == idFornecedor);
            return View(objResg);
        }

        //Realiza a exclusão dos dados selecionados. Irá cair nas excessões de erros os dados que já tem algum vinculo com alguma outra tabela, por exemplo: Fornecedor
        //que já possui pedido realizado por algum Funcionário
        [HttpGet]
        public ActionResult DeletarFornecedorConfir()
        {
            try
            {
                banco objBanco = new banco();
                tb_fornecedor objResg = objBanco.tb_fornecedor.Find(UtilController.idFornecedor);

                objBanco.tb_fornecedor.Remove(objResg);
                objBanco.SaveChanges();

                return Redirect("~/Fornecedor/ConsultarFornecedor");
            }
            catch
            {
                return HttpNotFound("Não foi possível realizar a exclusão!!");
            }
            
        }
    }
}