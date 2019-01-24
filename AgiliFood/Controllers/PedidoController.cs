using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgiliFood.Models;
using AgiliFood.Controllers;

namespace AgiliFood.Controllers
{
    public class PedidoController : Controller
    {

        // Pedido : Funcionário

        //Retorna uma lista de todos os Fornecedores ativos
        [HttpGet]
        public ActionResult SelecionarFornecedorPedido()
        {
            try
            {
                banco objBanco = new banco();
                List<tb_fornecedor> lstFornecedor = objBanco.tb_fornecedor.Where(p => p.status_fornecedor == true).ToList();
                return View(lstFornecedor);
            }
            catch
            {
                return HttpNotFound("Ocorreu um erro na operação");
            }
            
        }

        //Retorna uma lista de todos os produtos ativos, respectivamente, do idFornecedor enviado da View "SelecionarFornecedorPedido"
        [HttpGet]
        public ActionResult SelecionarProdutoPedido(int idFornecedor)
        {
            UtilController.idFornecedor = idFornecedor;

            banco objBanco = new banco();
            List<tb_produtos> lstProdutos = objBanco.tb_produtos.Where(p => p.id_fornecedor == idFornecedor && p.status_produtos == true).ToList();

            if(lstProdutos.Count == 0)
            {
                ModelState.AddModelError("Error", "Fornecedor não contém produtos disponíveis");
                return View(lstProdutos);
            }
            else
            {
                return View(lstProdutos);
            }
            
        }
            
        //Retorna a View "RealizarPedido"
        [HttpGet]
        public ActionResult RealizarPedido(int idProduto)
        {
            UtilController.idProduto = idProduto;
            return View();
        }

        //Método criado para gravar no banco o pedido realizado pelo Funcionário logado
        [HttpPost]
        public ActionResult RealizarPedido(tb_pedido objPedido)
        {
            try
            {
                if (objPedido.quantidade_produto <= 0)
                {
                    ModelState.AddModelError("Error", "Quantidade inválida, selecione maior ou igual a 1.");
                    return View();
                }
                else
                {
                    banco objBanco = new banco();
                    objPedido.id_funcionario = UtilController.idFuncionario;
                    objPedido.id_fornecedor = UtilController.idFornecedor;
                    objPedido.id_produto = UtilController.idProduto;
                    objPedido.date_pedido = DateTime.Now;

                    objBanco.tb_pedido.Add(objPedido);
                    ModelState.Clear();

                    ModelState.AddModelError("Success", "Pedido realizado com sucesso.");

                    objBanco.SaveChanges();

                    return View();
                }
            }
            catch 
            {
                ModelState.AddModelError("Error", "Preencher todo(s) o(s) campo(s).");
                return View();
            }   
        }

        //Pedido : Rh/Financeiro

        //Retorna a View "ConsultarFuncionarioPedido" com uma lista de todos os Funcionários cadastrados
        [HttpGet]
        public ActionResult ConsultarFuncionarioPedido()
        {
            try
            {
                banco objBanco = new banco();
                List<tb_funcionario> lstFuncionario = objBanco.tb_funcionario.ToList();

                if (lstFuncionario.Count == 0)
                {
                    ModelState.AddModelError("Error", "Não possui funcionários cadastrados");
                    return View(lstFuncionario);
                }

                else
                {
                    return View(lstFuncionario);
                }
               
            }
            catch
            {
                return HttpNotFound("Ocorreu um erro na operação");
            }
            
        }

        //Retorna a View "FiltrarDataPedido"
        [HttpGet]
        public ActionResult FiltrarDataPedido(int idFuncionario)
        {
            UtilController.idFuncionario = idFuncionario;
            return View();
        }

        //Método que irá receber as datas referentes ao filtro de pedidos, assim, direcionando para a View "ListaPedidoRealizado"
        [HttpPost]
        public ActionResult FiltrarDataPedido(string txtDataInicial, string txtDataFinal)
        {
            UtilController.dataInicial = Convert.ToDateTime(txtDataInicial);
            UtilController.dataFinal = Convert.ToDateTime(txtDataFinal);
            return Redirect("~/Pedido/ListaPedidoRealizado");
        }

        //Retorna a View "ListaPedidoRealizado" com uma lista de pedidos realizados no período informado na View "FiltrarDataPedido"
        [HttpGet]
        public ActionResult ListaPedidoRealizado()
        {
            try
            {
                banco objBanco = new banco();
                List<tb_pedido> lstPedido = objBanco.tb_pedido.Where(p => p.date_pedido >= UtilController.dataInicial && p.date_pedido <= UtilController.dataFinal
                                             && p.id_funcionario == UtilController.idFuncionario).ToList();

                if (lstPedido.Count == 0)
                {
                    ModelState.AddModelError("Error", "Funcionário não possui pedidos realizados neste período");
                    return View(lstPedido);
                }
                else
                {
                    return View(lstPedido);
                }
            }
            catch
            {
                return HttpNotFound("Ocorreu um erro na operação");
            }
            
        }
    }
}