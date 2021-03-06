﻿using System;
using System.Linq;
using NHibernate;
using Sicemed.Web.Models.BI;

namespace Sicemed.Web.Infrastructure.Jobs
{
    public class CalculadorIndicadoresJob : NHibernateJob
    {
        public CalculadorIndicadoresJob()
            : base("Calcular Indicadores Job", TimeSpan.FromMinutes(1)) { }

        protected override void Run(ISession session)
        {
            var indicadoresHabilitados = session.QueryOver<Indicador>()
                    .Where(x => x.Habilitado)
                    .Fetch(x => x.Objetivos)
                    .Eager.List();
            indicadoresHabilitados.AsParallel().ForAll(indicador =>
                {
                    try
                    {
                        if (Log.IsInfoEnabled) Log.InfoFormat("Calculando el indicador '{0}) {1}'", indicador.Id, indicador.Nombre);
                        var fecha = DateTime.Now;

                        var denominadorQuery = session.CreateSQLQuery(indicador.DenominadorSql);
                        if(indicador.DenominadorSql.Contains(":Mes")) denominadorQuery.SetParameter("Mes", fecha.Month);
                        if(indicador.DenominadorSql.Contains(":Anio")) denominadorQuery.SetParameter("Anio", fecha.Year);                            
                        var denominador = denominadorQuery.UniqueResult<int>();

                        if (Log.IsDebugEnabled) Log.DebugFormat("Calculando denominador para el indicador '{0}) {1}': {2}", indicador.Id, indicador.Nombre, denominador);

                        var numeradorQuery = session.CreateSQLQuery(indicador.NumeradorSql);
                        if(indicador.NumeradorSql.Contains(":Mes")) numeradorQuery.SetParameter("Mes", fecha.Month);
                        if(indicador.NumeradorSql.Contains(":Anio")) numeradorQuery.SetParameter("Anio", fecha.Year);                        
                        var numerador = numeradorQuery.UniqueResult<int>();

                        if (Log.IsDebugEnabled) Log.DebugFormat("Calculando numerador para el indicador '{0}) {1}': {2}", indicador.Id, indicador.Nombre, numerador);

                        var valor = Convert.ToDecimal(numerador / denominador);

                        var objetivo = indicador.Objetivos.SingleOrDefault(o => o.Anio == fecha.Year && o.Mes == fecha.Month);

                        if (objetivo == null)
                        {
                            objetivo = new ObjetivoIndicador
                            {
                                Anio = fecha.Year,
                                Mes = fecha.Month,
                                ValorMaximo = valor,
                                ValorMinimo = valor,
                                Indicador = indicador
                            };
                            indicador.AgregarObjetivo(objetivo);
                        }
                        objetivo.FechaLectura = fecha;
                        objetivo.Valor = valor;

                        session.Save(objetivo);

                        if (Log.IsInfoEnabled) Log.InfoFormat("Indicador '{0}) {1}' calculado con valor: {2}", indicador.Id, indicador.Nombre, valor);
                    }
                    catch (Exception ex)
                    {
                        Log.ErrorFormat("Hubo un error al calcular el indicador '{0}) {1}'. Exc: {2}", indicador.Id, indicador.Nombre, ex);
                    }
                });
        }
    }
}