using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgiliFood.Models;

namespace AgiliFood.Controllers
{
    public class ProdutosController : Controller
    {
        //Retorna a View "SelecionarFornecedor" com todos Fornecedores cadastrados na tabela "tb_fornecedor"
        [HttpGet]
        public ActionResult SelecionarFornecedor()
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
                return HttpNotFound("Ocorreu um erro na operação");
            }
        }

        //Recebe o idFornecedor da View "SelecionarFornecedor" e busca no banco todos os produtos cadastros por este Fornecedor selecionado.
        [HttpGet]
        public ActionResult ConsultarProdutos(int idFornecedor)
        {
            try
            {
                UtilController.idFornecedor = idFornecedor;
                banco objBanco = new banco();
                List<tb_produtos> lstProdutos = objBanco.tb_produtos.Where(p => p.id_fornecedor == UtilController.idFornecedor).ToList();

                if (lstProdutos.Count == 0)
                {
                    ModelState.AddModelError("Error", "Não possui produtos cadastrados.");
                    return View(lstProdutos);
                }

                else
                {
                    return View(lstProdutos);
                }
            }
            catch
            {
                return HttpNotFound("Ocorreu um erro na operação");
            }
        }

        //Retorna a View "InserirProdutos"
        [HttpGet]
        public ActionResult InserirProdutos()
        {
            return View();
        }

        //Método criado para gravar os dados da View "InserirProdutos" no banco de dados na tabela "tb_produtos"
        [HttpPost]
        public ActionResult InserirProdutos(tb_produtos objProdutos)
        {
            if (objProdutos.nome_produto == null)
            {
                ModelState.AddModelError("Error", "Preecher todo(s) o(s) campo(s). *Exceto o campo ATIVO*");
                return View();
            }

            else
            {
                try
                {
                    banco objBanco = new banco();
                    objProdutos.id_fornecedor = UtilController.idFornecedor;
                    objBanco.tb_produtos.Add(objProdutos);
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

        //Recebe o idProduto da View "ConsultarProdutos", faz uma busca no banco, e retorna todos os dados referentes ao idProduto na View "EditarProdutos"
        [HttpGet]
        public ActionResult EditarProdutos(int idProduto)
        {
            banco objBanco = new banco();
            tb_produtos objResg = objBanco.tb_produtos.Find(idProduto);
            return View(objResg);
        }

        //Método criado para gravar os dados da View "EditarProdutos" no banco de dados na tabela "tb_produtos" referente ao mesmo idProduto recebido
        //da View "ConsultarProdutos"
        [HttpPost]
        public ActionResult EditarProdutos(tb_produtos objProdutos)
        {
            try
            {
                banco objBanco = new banco();
                tb_produtos objResg = objBanco.tb_produtos.FirstOrDefault(p => p.id_produto == objProdutos.id_produto);

                int codFornecedor = objProdutos.id_fornecedor;

                objResg.nome_produto = objProdutos.nome_produto;
                objResg.status_produtos = objProdutos.status_produtos;

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

        //Retorna a View "DeletarProdutos" com os dados referente ao idProduto recebido da View "ConsultarProdutos"
        [HttpGet]
        public ActionResult DeletarProdutos(int idProduto)
        {
            UtilController.idProduto = idProduto;

            banco objBanco = new banco();
            tb_produtos objResg = objBanco.tb_produtos.FirstOrDefault(p => p.id_produto == idProduto);
            return View(objResg);
        }

        //Realiza a exclusão dos dados selecionados. Irá cair nas excessões de erros os dados que já tem algum vinculo com alguma outra tabela, por exemplo: Produto
        //que já possui pedido realizado por algum Funcionário
        [HttpGet]
        public ActionResult DeletarProdutosConfir()
        {
            try
            {
                banco objBanco = new banco();
                tb_produtos objResg = objBanco.tb_produtos.Find(UtilController.idProduto);

                objBanco.tb_produtos.Remove(objResg);
                objBanco.SaveChanges();

                return Redirect("~/Produtos/SelecionarFornecedor");
            }
            catch
            {
                return HttpNotFound("Não foi possível realizar a exclusão!!"); 
            }
            
        }
    }
}