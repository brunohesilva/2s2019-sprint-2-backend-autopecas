﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Senai.AutoPecas.WebApi.Domains;
using Senai.AutoPecas.WebApi.Interfaces;
using Senai.AutoPecas.WebApi.Repositories;

namespace Senai.AutoPecas.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class PecasController : ControllerBase
    {
        private IPecaRepository PecaRepository { get; set; }

        public PecasController()
        {
            PecaRepository = new PecaRepository();
        }

        [HttpPost]
        public IActionResult Cadastrar(Pecas peca)
        {
            try
            {
                int FornecedorId = Convert.ToInt32(HttpContext.User.Claims.First(x => x.Type == JwtRegisteredClaimNames.Jti).Value);
                peca.FornecedorId = FornecedorId;
                PecaRepository.Cadastrar(peca);
                return Ok();
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult Listar()
        {
            return Ok(PecaRepository.Listar());
        }

        [HttpGet("{id}")]
        public IActionResult BuscarPorId(int id)
        {
            try
            {
                Pecas peca = PecaRepository.BuscarPorId(id);
                if (peca == null)
                    return NotFound();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = ex.Message });
            }
        }

        [HttpPut]
        public IActionResult Atualizar(Pecas peca)
        {
            try
            {
                Pecas PecaBuscada = PecaRepository.BuscarPorId(peca.PecaId);
                if (PecaBuscada == null)
                    return NotFound();

                PecaRepository.Atualizar(peca);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            PecaRepository.Deletar(id);
            return Ok();
        }
    }
}