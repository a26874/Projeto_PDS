/*
*	<copyright file="StatusFoto" company="IPCA">
*	</copyright>
* 	<author>Marco Macedo</author>
*	<contact>a26874@alunos.ipca.pt</contact>
*   <date>2024 4/14/2024 4:11:43 PM</date>
*	<description></description>
**/

using System.ComponentModel.DataAnnotations;

namespace ProjetoPDS.Classes
{
    public enum StatusFoto
    {
        COMPLETO,
        PROCESSANDO,
        ERRO
    }
}