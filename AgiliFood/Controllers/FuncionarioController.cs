using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgiliFood.Models;

namespace AgiliFood.Controllers
{
    public class FuncionarioController : Controller
    {
        //Busca no banco todos os dados da tabela "tb_funcionario" e retorna na View "ConsultarFuncionario"
        [HttpGet]
        public ActionResult ConsultarFuncionario()
        {
            try
            {
                banco objBanco = new banco();
                List<tb_funcionario> lstFuncionario = objBanco.tb_funcionario.Where(p => p.id_rh == UtilController.idRh).ToList();

                if (lstFuncionario.Count == 0)
                {
                    ModelState.AddModelError("Error", "Não possui funcionários cadastrados.");
                    return View(lstFuncionario);
                }

                else
                {
                    return View(lstFuncionario);
                }
            }

            catch
            {
                return HttpNotFound("Ocorreu um erro na operação.");
            }
           
        }

        //Retorna a View "InserirFuncionario"
        [HttpGet]
        public ActionResult InserirFuncionario()
        {
            return View();
        }

        //Método criado para gravar os dados da View "InserirFuncionario" no banco de dados na tabela "tb_funcionario"
        [HttpPost]
        public ActionResult InserirFuncionario(tb_funcionario objFuncionario)
        {
            if (objFuncionario.nome_funcionario == null | objFuncionario.login_funcionario == null | objFuncionario.senha_funcionario == null)
            {
                ModelState.AddModelError("Error", "Preecher todo(s) o(s) campo(s).");
                return View();
            }

            else
            {
                tb_funcionario objResg = new banco().tb_funcionario.FirstOrDefault(p => p.login_funcionario == objFuncionario.login_funcionario);
                if (objResg != null)
                {
                    ModelState.AddModelError("Error", "Login já cadastrado, utilize outro.");
                    return View();
                }

                else
                {
                    try
                    {
                        banco objBanco = new banco();
                        objFuncionario.id_rh = UtilController.idRh;
                        objBanco.tb_funcionario.Add(objFuncionario);
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
        }

        //Recebe o idFuncionario da View "ConsultarFuncionario", faz uma busca no banco, e retorna todos os dados referentes ao idFuncionario na View "EditarFuncionario"
        [HttpGet]
        public ActionResult EditarFuncionario(int idFuncionario)
        {
            banco objBanco = new banco();
            tb_funcionario objResg = objBanco.tb_funcionario.Find(idFuncionario);
            return View(objResg);
        }

        //Método criado para gravar os dados da View "EditarFuncionario" no banco de dados na tabela "tb_funcionario" referente ao mesmo idFuncionario recebido
        //da View "ConsultarFuncionario"
        [HttpPost]
        public ActionResult EditarFuncionario(tb_funcionario objFuncionario)
        {
            try
            {
                banco objBanco = new banco();
                tb_funcionario objResg = objBanco.tb_funcionario.FirstOrDefault(p => p.id_funcionario == objFuncionario.id_funcionario);

                objResg.nome_funcionario = objFuncionario.nome_funcionario;
                objResg.login_funcionario = objFuncionario.login_funcionario;
                objResg.senha_funcionario = objFuncionario.senha_funcionario;

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

        //Retorna a View "DeletarFuncionario" com os dados referente ao idFuncionario recebido da View "ConsultarFuncionario"
        [HttpGet]
        public ActionResult DeletarFuncionario(int idFuncionario)
        {
            UtilController.idFuncionario = idFuncionario;
            banco objBanco = new banco();
            tb_funcionario objResg = objBanco.tb_funcionario.FirstOrDefault(p => p.id_funcionario == idFuncionario);
            return View(objResg);
        }

        //Realiza a exclusão dos dados selecionados. Irá cair nas excessões de erros os dados que já tem algum vinculo com alguma outra tabela, por exemplo: Funcionário
        //que já possui pedido realizado
        [HttpGet]
        public ActionResult DeletarFuncionarioConfir()
        {
            try
            {
                banco objBanco = new banco();
                tb_funcionario objResg = objBanco.tb_funcionario.Find(UtilController.idFuncionario);

                objBanco.tb_funcionario.Remove(objResg);
                objBanco.SaveChanges();

                return Redirect("~/Funcionario/ConsultarFuncionario/");
            }
            catch
            {
                return HttpNotFound("Não é possível realizar a exclusão!!");
            }
        }
    }
}