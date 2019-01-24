using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AgiliFood.Models;
using AgiliFood.Controllers;

namespace AgiliFood.Controllers
{
    public class LoginController : Controller
    {
        //Retorna a View "LoginFuncionario"
        [HttpGet]
        public ActionResult LoginFuncionario()
        {
            if (UtilController.idFuncionario == 0)
            {
                return View();
            }

            else
            {
                return Redirect("~/Pedido/SelecionarFornecedorPedido");

            }
        }

        //Método que busca no banco se o login e a senha são compatíveis a funcionários já cadastrados, caso não retorne nenhum funcionário, o sistema retorna uma mensagem
        //para o usuário informando que o login e senha são inválidos, caso contrário, a classe estática idFuncionario recebe o "id_funcionario" referente ao login e senha
        //inseridos
        [HttpPost]
        public ActionResult LoginFuncionario(tb_funcionario objFun)
        {
            banco objBanco = new banco();
            tb_funcionario objResg = objBanco.tb_funcionario.FirstOrDefault(p => p.login_funcionario == objFun.login_funcionario && p.senha_funcionario == objFun.senha_funcionario);

            try
            {
                if(objFun.login_funcionario == null | objFun.senha_funcionario == null)
                {
                    ModelState.AddModelError("Error", "Preencher todo(s) o(s) campo(s).");
                    return View();
                }

                else
                {
                    if (objResg == null)
                    {
                        ModelState.AddModelError("Error", "Login e senha inválidos.");
                        return View();
                    }

                    else
                    {
                        UtilController.idFuncionario = objResg.id_funcionario;
                        return Redirect("~/Pedido/SelecionarFornecedorPedido");
                    }
                }
            }
            catch
            {
                ModelState.AddModelError("Error", "Ocorreu um erro na operação.");
                return View();
            }
            
        }

        //Retorna a View "LoginRh"
        [HttpGet]
        public ActionResult LoginRh()
        {
            if(UtilController.idRh == 0)
            {
                return View();
            }
            else
            {
                return Redirect("~/Home/Menu");
            }
            
        }

        //Método que busca no banco se o login e a senha são compatíveis aos dados já pré-cadastrados, diretamente na tabela "tb_rh", caso o banco retorne null,
        //o sistema retorna uma mensagem informando que o login e senha estão inválidos
        [HttpPost]
        public ActionResult LoginRh(tb_rh objRh)
        {
            banco objBanco = new banco();
            tb_rh objResg = objBanco.tb_rh.FirstOrDefault(p=>p.senha_rh == objRh.senha_rh && p.login_rh == objRh.login_rh);

            try
            {
                if (objRh.login_rh == null | objRh.senha_rh == null)
                {
                    ModelState.AddModelError("Error", "Preencher todo(s) o(s) campo(s).");
                    return View();
                }

                else
                {
                    if (objResg == null)
                    {
                        ModelState.AddModelError("Error", "Login e senha inválidos.");
                        return View();
                    }

                    else
                    {
                        UtilController.idRh = objResg.id_rh;
                        return Redirect("~/Home/Menu");
                    }
                }
                
            }
            catch
            {
                ModelState.AddModelError("Error", "Ocorreu um erro na operação");
                return View();
            }
           
        }

        [HttpGet]
        public ActionResult Deslogar()
        {
            UtilController.idRh = 0;
            UtilController.idFuncionario = 0;
            return Redirect("~/Home/Index");
        }
    }
}