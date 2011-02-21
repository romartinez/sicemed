<?xml version="1.0" encoding="UTF-8"?>
<stylesheet version="1.0" xmlns:nbsf="NuevoBancoSantaFe" xmlns="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:include href="functions.xslt"/>
<xsl:param name="ResourcePath"/>
  <xsl:template match="/">
    <fo:root font-family="Times" font-size="10pt" xmlns:fo="http://www.w3.org/1999/XSL/Format">
      <fo:layout-master-set>
        <fo:simple-page-master master-name="barcodes-page" page-width="21cm" page-height="29.7cm">
          <fo:region-body margin-bottom="2cm" margin-top="2cm" margin-left="2cm" margin-right="2cm"/>
        </fo:simple-page-master>
      </fo:layout-master-set>
      <fo:page-sequence master-reference="barcodes-page">
        <fo:flow flow-name="xsl-region-body" font-family="Arial, Helvetica" font-size="10pt">
     
          <fo:block-container position="absolute" top="3.5cm" left="4.4cm" height="14pt" width="100%">
            <fo:block font-style="italic" color="black"><xsl:value-of select="/ROOT/JUEZ"/></fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="4cm" left="5.2cm" height="14pt" width="100%">
            <fo:block font-style="italic" color="black"><xsl:value-of select="/ROOT/SECRETARIO"/></fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="4.5cm" left="4.7cm" height="14pt" width="100%">
            <fo:block font-style="italic" color="black"><xsl:value-of select="/ROOT/JUICIO"/></fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="5.5cm" left="5cm" height="14pt" width="3.3cm">
            <fo:block font-style="italic" color="black"><xsl:value-of select="/ROOT/FECHADESDE/DIA"/>&#160;&#160;&#160;<xsl:value-of select="/ROOT/FECHADESDE/MES"/>&#160;&#160;&#160;&#160;<xsl:value-of select="/ROOT/FECHADESDE/ANIO"/></fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="5.5cm" left="9cm" height="14pt" width="3.3cm">
            <fo:block font-style="italic" color="black"><xsl:value-of select="/ROOT/FECHAHASTA/DIA"/>&#160;&#160;&#160;<xsl:value-of select="/ROOT/FECHAHASTA/MES"/>&#160;&#160;&#160;&#160;<xsl:value-of select="/ROOT/FECHAHASTA/ANIO"/></fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="6cm" left="4cm" height="14pt" width="100%">
            <fo:block font-style="italic" color="black"><xsl:value-of select="/ROOT/SOLICITADOPOR"/></fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="11.5cm" left="6.1cm" height="14pt" width="100%">
            <fo:block font-style="italic" color="black"><xsl:value-of select="/ROOT/SUCURSAL"/></fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="11.5cm" left="6.1cm" height="14pt" width="100%">
            <fo:block font-style="italic" color="black"><xsl:value-of select="/ROOT/SUCURSAL"/></fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="11.5cm" left="12.5cm" height="14pt" width="100%">
            <fo:block font-style="italic" color="black"><xsl:value-of select="/ROOT/AGENCIA"/></fo:block>
          </fo:block-container>
          
          <fo:block-container position="absolute" top="7.85cm" left="11.1cm" height="14pt" width="1.9cm">
            <fo:block font-style="italic" color="black" text-align="right"><xsl:value-of select="/ROOT/LIQUIDACION/DERECHOS"/></fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="8.45cm" left="11.1cm" height="14pt" width="1.9cm">
            <fo:block font-style="italic" color="black" text-align="right"><xsl:value-of select="/ROOT/LIQUIDACION/LEY4755"/></fo:block>
          </fo:block-container>
          <!--fo:block-container position="absolute" top="8.6cm" left="11.1cm" height="14pt" width="1.9cm">
            <fo:block font-style="italic" color="black" text-align="right"><xsl:value-of select="/ROOT/LIQUIDACION/COMISION"/></fo:block>
          </fo:block-container-->
          <fo:block-container position="absolute" top="9.65cm" left="11.1cm" height="14pt" width="1.9cm">
            <fo:block font-style="italic" font-weight="bold" color="black" text-align="right"><xsl:value-of select="/ROOT/LIQUIDACION/TOTAL"/></fo:block>
          </fo:block-container>

          <fo:block-container position="absolute" top="12.2cm" left="11.1cm" height="14pt" width="1.9cm">
            <fo:block font-style="italic" color="black" text-align="right"><xsl:value-of select="/ROOT/PAGOS/EFECTIVO"/></fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="12.8cm" left="11.1cm" height="14pt" width="1.9cm">
            <fo:block font-style="italic" color="black" text-align="right"><xsl:value-of select="/ROOT/PAGOS/CHEQUE"/></fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="13.4cm" left="11.1cm" height="14pt" width="1.9cm">
            <fo:block font-style="italic" color="black" text-align="right"><xsl:value-of select="/ROOT/PAGOS/COMISION"/></fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="14cm" left="11.1cm" height="14pt" width="1.9cm">
            <fo:block font-style="italic" font-weight="bold" color="black" text-align="right"><xsl:value-of select="/ROOT/PAGOS/TOTAL"/></fo:block>
          </fo:block-container>
          
          <fo:block-container position="absolute" top="15.85cm" left="4.3cm" height="16pt" width="23mm">
            <fo:block font-style="italic" color="black"><xsl:value-of select="/ROOT/FECHAEMISION/DIA"/></fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="15.85cm" left="7cm" height="16pt" width="40mm">
            <fo:block font-style="italic" color="black"><xsl:value-of select="/ROOT/FECHAEMISION/MES"/></fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="15.85cm" left="12.2cm" height="16pt" width="1.6cm">
            <fo:block font-style="italic" color="black"><xsl:value-of select="/ROOT/FECHAEMISION/ANIO"/></fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="14.8cm" left="5.5cm" height="18mm" width="8cm" line-height="150%">
			      <xsl:variable name="EnLetras" select="nbsf:NumeroALetras(/ROOT/PAGOS/TOTAL)" />
			      <fo:block font-style="italic" font-weight="bold" color="black" text-align="left"><xsl:value-of select="$EnLetras" /></fo:block>
          </fo:block-container>
          
          <fo:table table-layout="fixed" width="100%">
            <fo:table-column column-width="proportional-column-width(1)"/>
            <fo:table-column column-width="2mm"/>
            <fo:table-column column-width="2.5cm"/>
            <fo:table-column column-width="4.6cm"/>
            <fo:table-column column-width="1cm"/>
            <fo:table-column column-width="2cm"/>
            <fo:table-column column-width="5mm"/>
            <fo:table-column column-width="2mm"/>
            <fo:table-column column-width="proportional-column-width(1)"/>
            <fo:table-body>
              <fo:table-row>
                <fo:table-cell column-number="2">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block text-align="left">
                    <fo:inline font-family="Basset" font-style="normal" font-weight="bold" font-size="12pt">SERIE B </fo:inline>
                    <fo:external-graphic src="{$ResourcePath}\escudo_gris.bmp" height="1.56cm" width="1.25cm"/>
                  </fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="3">
                  <fo:block text-align="right" margin-right="0mm">
                    <fo:inline font-size="14pt" font-family="Typist" font-style="normal" font-weight="normal">N&#0176; <xsl:value-of select="/ROOT/NUMERO" /></fo:inline>
                  </fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
              </fo:table-row>
              <fo:table-row>
                <fo:table-cell column-number="2">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="5">
                  <fo:block text-align="center" space-before="4mm">
                    <fo:block font-size="18pt" font-family="Times,Helvetica" font-style="italic" font-weight="bold">BOLETIN OFICIAL</fo:block>
                    <fo:block font-size="9pt" font-style="italic" font-weight="normal">PUBLICACIONES</fo:block>
                  </fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
              </fo:table-row>
              <fo:table-row>
                <fo:table-cell column-number="2">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="6">
                  <fo:block text-align="left" space-before="4mm" line-height="150%" margin-left="2mm">
                    <fo:block>Juez<fo:leader leader-pattern="dots" leader-length="9.7cm"/></fo:block>
                    <fo:block>Secretario<fo:leader leader-pattern="dots" leader-length="8.8cm"/></fo:block>
                    <fo:block>Juicio<fo:leader leader-pattern="dots" leader-length="9.5cm"/></fo:block>
                    <fo:block><fo:leader leader-pattern="dots" leader-length="10.3cm"/></fo:block>
                    <fo:block>Desde el ....../......./..........  hasta el ......./......./............ a pedido de</fo:block>
                    <fo:block><fo:leader leader-pattern="dots" leader-length="10.3cm"/></fo:block>
                  </fo:block>
                </fo:table-cell>
              </fo:table-row>
              <fo:table-row>
                <fo:table-cell column-number="2">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="5">
                  <fo:block text-align="center" space-before="4mm" space-after="4mm">LIQUIDACION</fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
              </fo:table-row>
              <fo:table-row>
                <fo:table-cell column-number="2">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="3" border-left="1pt solid black" border-top="1pt solid black" padding-bottom="0mm" padding-top="1mm" height="5mm">
                  <fo:block text-align="left" margin-left="2mm">
                    Derechos <fo:leader leader-pattern="dots" leader-length="6.1cm"/>
                  </fo:block>
                </fo:table-cell>
                <fo:table-cell border="1px solid black">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell border="1px solid black">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
              </fo:table-row>
              <fo:table-row>
                <fo:table-cell column-number="2">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="3" border-left="1pt solid black" padding-bottom="0mm" padding-top="1mm" height="5mm">
                  <fo:block text-align="left" margin-left="2mm">
                    Ley 4755 N° <fo:leader leader-pattern="dots" leader-length="5.6cm"/>
                  </fo:block>
                </fo:table-cell>
                <fo:table-cell border="1px solid black">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell border="1px solid black">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
              </fo:table-row>
              <fo:table-row>
                <fo:table-cell column-number="2">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="3" border-left="1pt solid black" padding-bottom="0mm" padding-top="1mm" height="5mm">
                  <fo:block text-align="left" margin-left="2mm">
                    <fo:leader leader-pattern="dots" leader-length="7.7cm"/>
                  </fo:block>
                </fo:table-cell>
                <fo:table-cell border="1px solid black">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell border="1px solid black">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
              </fo:table-row>
              <fo:table-row>
                <fo:table-cell column-number="2">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="3" border-left="1pt solid black" border-bottom="1pt solid black" padding-bottom="0mm" padding-top="1mm" height="5mm">
                  <fo:block text-align="right" margin-right="2mm">TOTAL $</fo:block>
                </fo:table-cell>
                <fo:table-cell border="1px solid black">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell border="1px solid black">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
              </fo:table-row>
              <fo:table-row>
                <fo:table-cell number-columns-spanned="7" column-number="2">
                  <fo:block text-align="center" space-before="4mm" font-weight="bold" font-style="italic" font-size="14pt">Nuevo Banco de Santa Fe S. A.</fo:block>
                </fo:table-cell>
              </fo:table-row>
              <fo:table-row>
                <fo:table-cell column-number="2">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="2">
                  <fo:block text-align="left" space-before="4mm" margin-left="2mm" line-height="150%">
                    <fo:block>Sucursal o casa <fo:leader leader-pattern="dots" leader-length="4.1cm"/>
                    </fo:block>
                  </fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="4">
                  <fo:block text-align="right" space-before="4mm" margin-right="2mm" line-height="150%">
                    <fo:block>Agencia N° <fo:leader leader-pattern="dots" leader-length="1.5cm"/>
                    </fo:block>
                  </fo:block>
                </fo:table-cell>
              </fo:table-row>
              <fo:table-row>
                <fo:table-cell column-number="2">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="3" border-left="1pt solid black" border-top="1pt solid black" padding-bottom="0mm" padding-top="1mm" height="5mm">
                  <fo:block text-align="left" margin-left="2mm">Depósito en efectivo <fo:leader leader-pattern="dots" leader-length="4.5cm"/>
                  </fo:block>
                </fo:table-cell>
                <fo:table-cell border="1px solid black">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell border="1px solid black">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
              </fo:table-row>
              <fo:table-row>
                <fo:table-cell column-number="2">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="3" border-left="1pt solid black" padding-bottom="0mm" padding-top="1mm" height="5mm">
                  <fo:block text-align="left" margin-left="2mm">Cheque Giro N° <fo:leader leader-pattern="dots" leader-length="5.1cm"/>
                  </fo:block>
                </fo:table-cell>
                <fo:table-cell border="1px solid black">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell border="1px solid black">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
              </fo:table-row>
              <fo:table-row>
                <fo:table-cell column-number="2">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="3" border-left="1pt solid black" padding-bottom="0mm" padding-top="1mm" height="5mm">
                  <fo:block text-align="left" margin-left="2mm">c/Banco <fo:leader leader-pattern="dots" leader-length="6.3cm"/>
                  </fo:block>
                </fo:table-cell>
                <fo:table-cell border="1px solid black">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell border="1px solid black">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
              </fo:table-row>
              <fo:table-row>
                <fo:table-cell column-number="2">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="3" border-left="1pt solid black" border-bottom="1pt solid black" padding-bottom="0mm" padding-top="1mm" height="5mm">
                  <fo:block text-align="right" margin-right="2mm">TOTAL $</fo:block>
                </fo:table-cell>
                <fo:table-cell border="1px solid black">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell border="1px solid black">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
              </fo:table-row>
              <fo:table-row>
                <fo:table-cell column-number="2">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="5">
                  <fo:block text-align="left" line-height="150%" space-before="4mm" margin-left="2mm">
                    <fo:block>Son pesos.........................................................................................</fo:block>
                  </fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
              </fo:table-row>
              <fo:table-row>
                <fo:table-cell column-number="2">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="5">
                  <fo:block text-align="left" line-height="150%" margin-left="2mm">
                    <fo:block>
                      <fo:leader leader-pattern="dots" leader-length="10.5cm"/>
                    </fo:block>
                  </fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
              </fo:table-row>
              <fo:table-row>
                <fo:table-cell column-number="2">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="5">
                  <fo:block text-align="left" line-height="150%" margin-left="2mm">
                    <fo:block>.........................de.....................................................de ...................</fo:block>
                  </fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
              </fo:table-row>
              <fo:table-row>
                <fo:table-cell number-columns-spanned="7" column-number="2">
                  <fo:block text-align="center" space-before="4mm" space-after="4mm">
                    <fo:block font-size="36pt" font-family="IntHrP72DlTt"><xsl:value-of select="nbsf:ConvertTo2of5(/ROOT/CODIGOBARRA)" /></fo:block>
                  </fo:block>
                </fo:table-cell>
              </fo:table-row>
            </fo:table-body>
          </fo:table>
        </fo:flow>
      </fo:page-sequence>
    </fo:root>
  </xsl:template>
</stylesheet>
