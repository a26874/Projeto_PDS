/*
*	<copyright file="LocalPublicacao" company="IPCA">
*	</copyright>
* 	<author>Marco Macedo</author>
*	<contact>a26874@alunos.ipca.pt</contact>
*   <date>2024 4/14/2024 4:01:01 PM</date>
*	<description></description>
**/

using System.ComponentModel.DataAnnotations;

namespace ProjetoPDS.Classes
{
    /// <summary>
    /// Enumeração para o local de publicação.
    /// </summary>
    public enum LocalPublicacao
    {
        EncEduc,
        Sala,
        Mural,
        Chat
    }
}