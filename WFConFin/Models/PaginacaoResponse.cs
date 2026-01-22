namespace WFConFin.Models
{
    public class PaginacaoResponse<T> where T : class // ela pode ser qualquer classe
    {
        public PaginacaoResponse(IEnumerable<T> dados, long totalLinhas, int skip, int take)
        {
            Dados = dados;
            TotalLinhas = totalLinhas;
            Skip = skip;
            Take = take;
        }

        public IEnumerable<T> Dados { get; set; } // lista de itens do tipo T. lista dos dados da consulta em si
        public long TotalLinhas { get; set; } // total de linhas da consulta
        public int Skip { get; set; } // quantos registros foram ignorados
        public int Take { get; set; } // quantos registros foram trazidos


    }
}
