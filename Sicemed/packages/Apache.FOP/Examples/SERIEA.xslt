<?xml version="1.0" encoding="UTF-8"?>
<stylesheet version="1.0" xmlns:nbsf="NuevoBancoSantaFe" xmlns="http://www.w3.org/1999/XSL/Transform" xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
<xsl:include href="functions.xslt"/>
<xsl:param name="ResourcePath"/>
  <xsl:template match="/">
    
    <fo:root font-family="Times" font-size="10pt" xmlns:fo="http://www.w3.org/1999/XSL/Format">
      <fo:layout-master-set>
        <fo:simple-page-master master-name="barcodes-page" page-width="29.7cm" page-height="21cm">
          <fo:region-body margin-bottom="1cm" margin-top="2cm" margin-left="2cm" margin-right="2cm"/>
        </fo:simple-page-master>
      </fo:layout-master-set>
      <fo:page-sequence master-reference="barcodes-page">
        <fo:flow flow-name="xsl-region-body" font-family="Arial, Helvetica" font-size="10pt">

          <!-- INICIO DE LAS BOLETAS ORIGINALES -->
          <!-- boleta 1 -->
          <fo:block-container position="absolute" top="3.5cm" left="2.5cm" height="14pt" width="100%">
            <fo:block font-style="italic" color="black">
              <xsl:value-of select="/ROOT/SUSCRIPTOR"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="4cm" left="2.5cm" height="14pt" width="100%">
            <fo:block font-style="italic" color="black">
              <xsl:value-of select="/ROOT/DOMICILIO"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="4.5cm" left="4.6cm" height="14pt" width="100%">
            <fo:block font-style="italic" color="black">
              <xsl:value-of select="/ROOT/TERMINO"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="5cm" left="2.5cm" height="14pt" width="3.3cm">
            <fo:block font-style="italic" color="black">
              &#160;<xsl:value-of select="/ROOT/FECHADESDE/DIA"/>&#160;&#160;&#160;&#160;&#160;&#160;<xsl:value-of select="/ROOT/FECHADESDE/MES"/>&#160;&#160;&#160;&#160;&#160;&#160;&#160;<xsl:value-of select="/ROOT/FECHADESDE/ANIO"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="5cm" left="7.4cm" height="14pt" width="3.3cm">
            <fo:block font-style="italic" color="black">
              &#160;<xsl:value-of select="/ROOT/FECHAHASTA/DIA"/>&#160;&#160;&#160;&#160;&#160;&#160;<xsl:value-of select="/ROOT/FECHAHASTA/MES"/>&#160;&#160;&#160;&#160;&#160;&#160;&#160;<xsl:value-of select="/ROOT/FECHAHASTA/ANIO"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="6.4cm" left="0.6cm" height="1cm" width="10.1cm">
            <fo:block font-size="11pt" font-style="italic" text-align="center" color="black">
              <xsl:value-of select="/ROOT/DIRECCIONAREMITIR/LINEA1"/>
            </fo:block>
            <fo:block font-size="11pt" font-style="italic" text-align="center" color="black">
              <xsl:value-of select="/ROOT/DIRECCIONAREMITIR/LINEA2"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="8.3cm" left="3.4cm" height="14pt" width="100%">
            <fo:block font-style="italic" color="black">
              <xsl:value-of select="/ROOT/SUCURSAL"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="8.3cm" left="9.8cm" height="14pt" width="100%">
            <fo:block font-style="italic" color="black">
              <xsl:value-of select="/ROOT/AGENCIA"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="8.95cm" left="8.4cm" height="14pt" width="1.9cm">
            <fo:block font-style="italic" color="black" text-align="right">
              <xsl:value-of select="/ROOT/PAGOS/EFECTIVO"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="9.55cm" left="8.4cm" height="14pt" width="1.9cm">
            <fo:block font-style="italic" color="black" text-align="right">
              <xsl:value-of select="/ROOT/PAGOS/CHEQUE"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="10.15cm" left="8.4cm" height="14pt" width="1.9cm">
            <fo:block font-style="italic" color="black" text-align="right">
              <xsl:value-of select="/ROOT/PAGOS/COMISION"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="10.75cm" left="8.4cm" height="14pt" width="1.9cm">
            <fo:block font-style="italic" font-weight="bold" color="black" text-align="right">
              <xsl:value-of select="/ROOT/PAGOS/TOTAL"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="12.45cm" left="1.6cm" height="16pt" width="23mm">
            <fo:block font-style="italic" color="black">
              <xsl:value-of select="/ROOT/FECHAEMISION/DIA"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="12.45cm" left="4.3cm" height="16pt" width="40mm">
            <fo:block font-style="italic" color="black">
              <xsl:value-of select="/ROOT/FECHAEMISION/MES"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="12.45cm" left="9.5cm" height="16pt" width="1.6cm">
            <fo:block font-style="italic" color="black">
              <xsl:value-of select="/ROOT/FECHAEMISION/ANIO"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="11.4cm" left="2.8cm" height="18mm" width="8cm" line-height="150%">
            <xsl:variable name="EnLetras" select="nbsf:NumeroALetras(/ROOT/PAGOS/TOTAL)" />
            <fo:block font-style="italic" font-weight="bold" color="black" text-align="left">
              <xsl:value-of select="$EnLetras" />
            </fo:block>
          </fo:block-container>

          <!-- boleta 2 -->
          <fo:block-container position="absolute" top="3.5cm" left="17.5cm" height="14pt" width="100%">
            <fo:block font-style="italic" color="black">
              <xsl:value-of select="/ROOT/SUSCRIPTOR"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="4cm" left="17.5cm" height="14pt" width="100%">
            <fo:block font-style="italic" color="black">
              <xsl:value-of select="/ROOT/DOMICILIO"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="4.5cm" left="19.6cm" height="14pt" width="100%">
            <fo:block font-style="italic" color="black">
              <xsl:value-of select="/ROOT/TERMINO"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="5cm" left="17.5cm" height="14pt" width="3.3cm">
            <fo:block font-style="italic" color="black">
              &#160;<xsl:value-of select="/ROOT/FECHADESDE/DIA"/>&#160;&#160;&#160;&#160;&#160;&#160;<xsl:value-of select="/ROOT/FECHADESDE/MES"/>&#160;&#160;&#160;&#160;&#160;&#160;&#160;<xsl:value-of select="/ROOT/FECHADESDE/ANIO"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="5cm" left="22.4cm" height="14pt" width="3.3cm">
            <fo:block font-style="italic" color="black">
              &#160;<xsl:value-of select="/ROOT/FECHAHASTA/DIA"/>&#160;&#160;&#160;&#160;&#160;&#160;<xsl:value-of select="/ROOT/FECHAHASTA/MES"/>&#160;&#160;&#160;&#160;&#160;&#160;&#160;<xsl:value-of select="/ROOT/FECHAHASTA/ANIO"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="6.4cm" left="15.6cm" height="1cm" width="10.1cm">
            <fo:block font-size="11pt" font-style="italic" text-align="center" color="black">
              <xsl:value-of select="/ROOT/DIRECCIONAREMITIR/LINEA1"/>
            </fo:block>
            <fo:block font-size="11pt" font-style="italic" text-align="center" color="black">
              <xsl:value-of select="/ROOT/DIRECCIONAREMITIR/LINEA2"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="8.3cm" left="18.4cm" height="14pt" width="100%">
            <fo:block font-style="italic" color="black">
              <xsl:value-of select="/ROOT/SUCURSAL"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="8.3cm" left="24.8cm" height="14pt" width="100%">
            <fo:block font-style="italic" color="black">
              <xsl:value-of select="/ROOT/AGENCIA"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="8.95cm" left="23.4cm" height="14pt" width="1.9cm">
            <fo:block font-style="italic" color="black" text-align="right">
              <xsl:value-of select="/ROOT/PAGOS/EFECTIVO"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="9.55cm" left="23.4cm" height="14pt" width="1.9cm">
            <fo:block font-style="italic" color="black" text-align="right">
              <xsl:value-of select="/ROOT/PAGOS/CHEQUE"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="10.15cm" left="23.4cm" height="14pt" width="1.9cm">
            <fo:block font-style="italic" color="black" text-align="right">
              <xsl:value-of select="/ROOT/PAGOS/COMISION"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="10.75cm" left="23.4cm" height="14pt" width="1.9cm">
            <fo:block font-style="italic" font-weight="bold" color="black" text-align="right">
              <xsl:value-of select="/ROOT/PAGOS/TOTAL"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="12.45cm" left="16.6cm" height="16pt" width="23mm">
            <fo:block font-style="italic" color="black">
              <xsl:value-of select="/ROOT/FECHAEMISION/DIA"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="12.45cm" left="19.3cm" height="16pt" width="40mm">
            <fo:block font-style="italic" color="black">
              <xsl:value-of select="/ROOT/FECHAEMISION/MES"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="12.45cm" left="24.5cm" height="16pt" width="1.6cm">
            <fo:block font-style="italic" color="black">
              <xsl:value-of select="/ROOT/FECHAEMISION/ANIO"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="11.4cm" left="17.8cm" height="18mm" width="8cm" line-height="150%">
            <xsl:variable name="EnLetras" select="nbsf:NumeroALetras(/ROOT/PAGOS/TOTAL)" />
            <fo:block font-style="italic" font-weight="bold" color="black" text-align="left">
              <xsl:value-of select="$EnLetras" />
            </fo:block>
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

            <fo:table-column column-width="4cm"/>

            <fo:table-column column-width="2mm"/>
            <fo:table-column column-width="2.5cm"/>
            <fo:table-column column-width="4.6cm"/>
            <fo:table-column column-width="1cm"/>
            <fo:table-column column-width="2cm"/>
            <fo:table-column column-width="5mm"/>
            <fo:table-column column-width="2mm"/>
            <fo:table-column column-width="proportional-column-width(1)"/>
            <fo:table-body>
              <!-- row -->
              <fo:table-row>
                <!-- boleta 1 -->
                <fo:table-cell column-number="2">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block text-align="left">
                    <fo:inline font-family="Basset" font-style="normal" font-weight="bold" font-size="12pt">SERIE A </fo:inline>
                    <fo:external-graphic src="{$ResourcePath}\escudo_gris.bmp" height="1.56cm" width="1.25cm"/>
                  </fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="3">
                  <fo:block text-align="right" margin-right="0mm">
                    <fo:inline font-size="14pt" font-family="Typist" font-style="normal" font-weight="normal">
                      N&#0176; <xsl:value-of select="/ROOT/NUMERO" />
                    </fo:inline>
                  </fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>

                <!-- boleta 2 -->
                <fo:table-cell column-number="10">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block text-align="left">
                    <fo:inline font-family="Basset" font-style="normal" font-weight="bold" font-size="12pt">SERIE A </fo:inline>
                    <fo:external-graphic src="{$ResourcePath}\escudo_gris.bmp" height="1.56cm" width="1.25cm"/>
                  </fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="3">
                  <fo:block text-align="right" margin-right="0mm">
                    <fo:inline font-size="14pt" font-family="Typist" font-style="normal" font-weight="normal">
                      N&#0176; <xsl:value-of select="/ROOT/NUMERO" />
                    </fo:inline>
                  </fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
              </fo:table-row>

              <!-- row -->

              <fo:table-row>
                <fo:table-cell column-number="2">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="5">
                  <fo:block text-align="center" space-before="4mm">
                    <fo:block font-size="18pt" font-family="Times,Helvetica" font-style="italic" font-weight="bold">BOLETIN OFICIAL</fo:block>
                    <fo:block font-size="9pt" font-style="italic" font-weight="normal">SUSCRIPCIONES</fo:block>
                  </fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>

                <fo:table-cell column-number="10">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="5">
                  <fo:block text-align="center" space-before="4mm">
                    <fo:block font-size="18pt" font-family="Times,Helvetica" font-style="italic" font-weight="bold">BOLETIN OFICIAL</fo:block>
                    <fo:block font-size="9pt" font-style="italic" font-weight="normal">SUSCRIPCIONES</fo:block>
                  </fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
              </fo:table-row>

              <!-- row -->

              <fo:table-row>
                <fo:table-cell column-number="2">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="6">
                  <fo:block text-align="left" space-before="4mm" line-height="150%" margin-left="2mm">
                    <fo:block>Suscriptor........................................................................................</fo:block>
                    <fo:block>Domicilio.........................................................................................</fo:block>
                    <fo:block>Término de suscripción...................................................................</fo:block>
                    <fo:block>Desde el ........../.........../............  hasta el .........../.........../...............</fo:block>
                  </fo:block>
                </fo:table-cell>

                <fo:table-cell column-number="10">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="6">
                  <fo:block text-align="left" space-before="4mm" line-height="150%" margin-left="2mm">
                    <fo:block>Suscriptor........................................................................................</fo:block>
                    <fo:block>Domicilio.........................................................................................</fo:block>
                    <fo:block>Término de suscripción...................................................................</fo:block>
                    <fo:block>Desde el ........../.........../............  hasta el .........../.........../...............</fo:block>
                  </fo:block>
                </fo:table-cell>
              </fo:table-row>

              <!-- row -->

              <fo:table-row>
                <fo:table-cell column-number="2">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="5">
                  <fo:block text-align="center" space-before="2mm" space-after="2mm">DIRECCION A REMITIR EL BOLETIN</fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>

                <fo:table-cell column-number="10">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="5">
                  <fo:block text-align="center" space-before="2mm" space-after="2mm">DIRECCION A REMITIR EL BOLETIN</fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
              </fo:table-row>

              <!-- row -->

              <fo:table-row>
                <fo:table-cell column-number="2">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="5" border="1pt solid black" height="1cm">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>

                <fo:table-cell column-number="10">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="5" border="1pt solid black" height="1cm">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
              </fo:table-row>

              <!-- row -->

              <fo:table-row>
                <fo:table-cell number-columns-spanned="7" column-number="2">
                  <fo:block text-align="center" space-before="2mm" font-weight="bold" font-style="italic" font-size="14pt">Nuevo Banco de Santa Fe S. A.</fo:block>
                </fo:table-cell>

                <fo:table-cell number-columns-spanned="7" column-number="10">
                  <fo:block text-align="center" space-before="2mm" font-weight="bold" font-style="italic" font-size="14pt">Nuevo Banco de Santa Fe S. A.</fo:block>
                </fo:table-cell>
              </fo:table-row>

              <!-- row -->

              <fo:table-row>
                <fo:table-cell column-number="2">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="2">
                  <fo:block text-align="left" space-before="2mm" margin-left="2mm" line-height="150%">
                    <fo:block>
                      Sucursal o casa <fo:leader leader-pattern="dots" leader-length="4.1cm"/>
                    </fo:block>
                  </fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="4">
                  <fo:block text-align="right" space-before="2mm" margin-right="2mm" line-height="150%">
                    <fo:block>
                      Agencia N° <fo:leader leader-pattern="dots" leader-length="1.5cm"/>
                    </fo:block>
                  </fo:block>
                </fo:table-cell>

                <fo:table-cell column-number="10">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="2">
                  <fo:block text-align="left" space-before="2mm" margin-left="2mm" line-height="150%">
                    <fo:block>
                      Sucursal o casa <fo:leader leader-pattern="dots" leader-length="4.1cm"/>
                    </fo:block>
                  </fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="4">
                  <fo:block text-align="right" space-before="2mm" margin-right="2mm" line-height="150%">
                    <fo:block>
                      Agencia N° <fo:leader leader-pattern="dots" leader-length="1.5cm"/>
                    </fo:block>
                  </fo:block>
                </fo:table-cell>
              </fo:table-row>

              <!-- row -->

              <fo:table-row>
                <fo:table-cell column-number="2">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="3" border-left="1pt solid black" border-top="1pt solid black" padding-bottom="0mm" padding-top="1mm" height="5mm">
                  <fo:block text-align="left" margin-left="2mm">
                    Depósito en efectivo <fo:leader leader-pattern="dots" leader-length="4.5cm"/>
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

                <fo:table-cell column-number="10">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="3" border-left="1pt solid black" border-top="1pt solid black" padding-bottom="0mm" padding-top="1mm" height="5mm">
                  <fo:block text-align="left" margin-left="2mm">
                    Depósito en efectivo <fo:leader leader-pattern="dots" leader-length="4.5cm"/>
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

              <!-- row -->

              <fo:table-row>
                <fo:table-cell column-number="2">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="3" border-left="1pt solid black" padding-bottom="0mm" padding-top="1mm" height="5mm">
                  <fo:block text-align="left" margin-left="2mm">
                    Cheque Giro N° <fo:leader leader-pattern="dots" leader-length="5.1cm"/>
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

                <fo:table-cell column-number="10">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="3" border-left="1pt solid black" padding-bottom="0mm" padding-top="1mm" height="5mm">
                  <fo:block text-align="left" margin-left="2mm">
                    Cheque Giro N° <fo:leader leader-pattern="dots" leader-length="5.1cm"/>
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

              <!-- row -->

              <fo:table-row>
                <fo:table-cell column-number="2">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="3" border-left="1pt solid black" padding-bottom="0mm" padding-top="1mm" height="5mm">
                  <fo:block text-align="left" margin-left="2mm">
                    c/Banco <fo:leader leader-pattern="dots" leader-length="6.3cm"/>
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

                <fo:table-cell column-number="10">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="3" border-left="1pt solid black" padding-bottom="0mm" padding-top="1mm" height="5mm">
                  <fo:block text-align="left" margin-left="2mm">
                    c/Banco <fo:leader leader-pattern="dots" leader-length="6.3cm"/>
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

              <!-- row -->

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

                <fo:table-cell column-number="10">
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

              <!-- row -->

              <fo:table-row>
                <fo:table-cell column-number="2">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="5">
                  <fo:block text-align="left" line-height="150%" space-before="2mm" margin-left="2mm">Son pesos.........................................................................................</fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>

                <fo:table-cell column-number="10">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="5">
                  <fo:block text-align="left" line-height="150%" space-before="2mm" margin-left="2mm">Son pesos.........................................................................................</fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
              </fo:table-row>

              <!-- row -->

              <fo:table-row>
                <fo:table-cell column-number="2">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="5">
                  <fo:block text-align="left" line-height="150%" margin-left="2mm">
                    <fo:leader leader-pattern="dots" leader-length="10.5cm"/>
                  </fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>

                <fo:table-cell column-number="10">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="5">
                  <fo:block text-align="left" line-height="150%" margin-left="2mm">
                    <fo:leader leader-pattern="dots" leader-length="10.5cm"/>
                  </fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
              </fo:table-row>

              <!-- row -->

              <fo:table-row>
                <fo:table-cell column-number="2">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="5">
                  <fo:block text-align="left" line-height="150%" margin-left="2mm">.........................de.....................................................de ...................</fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>

                <fo:table-cell column-number="10">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="5">
                  <fo:block text-align="left" line-height="150%" margin-left="2mm">.........................de.....................................................de ...................</fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
              </fo:table-row>

              <!-- row -->

              <fo:table-row>
                <fo:table-cell number-columns-spanned="7" column-number="2">
                  <fo:block text-align="center" font-size="36pt" font-family="IntHrP72DlTt">
                    <xsl:value-of select="nbsf:ConvertTo2of5(/ROOT/CODIGOBARRA)" />
                  </fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="7" column-number="10">
                  <fo:block text-align="center" font-size="36pt" font-family="IntHrP72DlTt">
                    <xsl:value-of select="nbsf:ConvertTo2of5(/ROOT/CODIGOBARRA)" />
                  </fo:block>
                </fo:table-cell>
              </fo:table-row>

              <!-- row -->

              <fo:table-row>
                <fo:table-cell number-columns-spanned="7" column-number="2">
                  <fo:block text-align="center" width="100%" font-size="9pt">Para el contribuyente.</fo:block>
                  <fo:block font-style="italic" font-weight="bold" text-align="center" width="100%" font-size="9pt">&quot;Éste comprobante es válido para el pago en el día de la emisión&quot;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="7" column-number="10">
                  <fo:block text-align="center" width="100%" font-size="9pt">Para el Ministerio de Gobierno y Reforma del Estado.</fo:block>
                  <fo:block text-align="center" width="100%" font-size="8.25pt">Dirección General de la Imprenta Oficial - Boletín Oficial</fo:block>
                  <fo:block text-align="center" width="100%" font-size="8.25pt">Av. Vicente Peñaloza 5385 - 3000 Santa Fe</fo:block>
                </fo:table-cell>
              </fo:table-row>
            </fo:table-body>
          </fo:table>
          <!-- FIN DE LAS BOLETAS -->

          <fo:block break-after="page"/>

          <!-- COPIA DE LAS BOLETAS-->
          
          <!-- INICIO DE LAS BOLETAS -->
          <!-- boleta 1 -->
          <fo:block-container position="absolute" top="3.5cm" left="2.5cm" height="14pt" width="100%">
            <fo:block font-style="italic" color="black">
              <xsl:value-of select="/ROOT/SUSCRIPTOR"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="4cm" left="2.5cm" height="14pt" width="100%">
            <fo:block font-style="italic" color="black">
              <xsl:value-of select="/ROOT/DOMICILIO"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="4.5cm" left="4.6cm" height="14pt" width="100%">
            <fo:block font-style="italic" color="black">
              <xsl:value-of select="/ROOT/TERMINO"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="5cm" left="2.5cm" height="14pt" width="3.3cm">
            <fo:block font-style="italic" color="black">
              &#160;<xsl:value-of select="/ROOT/FECHADESDE/DIA"/>&#160;&#160;&#160;&#160;&#160;&#160;<xsl:value-of select="/ROOT/FECHADESDE/MES"/>&#160;&#160;&#160;&#160;&#160;&#160;&#160;<xsl:value-of select="/ROOT/FECHADESDE/ANIO"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="5cm" left="7.4cm" height="14pt" width="3.3cm">
            <fo:block font-style="italic" color="black">
              &#160;<xsl:value-of select="/ROOT/FECHAHASTA/DIA"/>&#160;&#160;&#160;&#160;&#160;&#160;<xsl:value-of select="/ROOT/FECHAHASTA/MES"/>&#160;&#160;&#160;&#160;&#160;&#160;&#160;<xsl:value-of select="/ROOT/FECHAHASTA/ANIO"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="6.4cm" left="0.6cm" height="1cm" width="10.1cm">
            <fo:block font-size="11pt" font-style="italic" text-align="center" color="black">
              <xsl:value-of select="/ROOT/DIRECCIONAREMITIR/LINEA1"/>
            </fo:block>
            <fo:block font-size="11pt" font-style="italic" text-align="center" color="black">
              <xsl:value-of select="/ROOT/DIRECCIONAREMITIR/LINEA2"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="8.3cm" left="3.4cm" height="14pt" width="100%">
            <fo:block font-style="italic" color="black">
              <xsl:value-of select="/ROOT/SUCURSAL"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="8.3cm" left="9.8cm" height="14pt" width="100%">
            <fo:block font-style="italic" color="black">
              <xsl:value-of select="/ROOT/AGENCIA"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="8.95cm" left="8.4cm" height="14pt" width="1.9cm">
            <fo:block font-style="italic" color="black" text-align="right">
              <xsl:value-of select="/ROOT/PAGOS/EFECTIVO"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="9.55cm" left="8.4cm" height="14pt" width="1.9cm">
            <fo:block font-style="italic" color="black" text-align="right">
              <xsl:value-of select="/ROOT/PAGOS/CHEQUE"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="10.15cm" left="8.4cm" height="14pt" width="1.9cm">
            <fo:block font-style="italic" color="black" text-align="right">
              <xsl:value-of select="/ROOT/PAGOS/COMISION"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="10.75cm" left="8.4cm" height="14pt" width="1.9cm">
            <fo:block font-style="italic" font-weight="bold" color="black" text-align="right">
              <xsl:value-of select="/ROOT/PAGOS/TOTAL"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="12.45cm" left="1.6cm" height="16pt" width="23mm">
            <fo:block font-style="italic" color="black">
              <xsl:value-of select="/ROOT/FECHAEMISION/DIA"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="12.45cm" left="4.3cm" height="16pt" width="40mm">
            <fo:block font-style="italic" color="black">
              <xsl:value-of select="/ROOT/FECHAEMISION/MES"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="12.45cm" left="9.5cm" height="16pt" width="1.6cm">
            <fo:block font-style="italic" color="black">
              <xsl:value-of select="/ROOT/FECHAEMISION/ANIO"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="11.4cm" left="2.8cm" height="18mm" width="8cm" line-height="150%">
            <xsl:variable name="EnLetras" select="nbsf:NumeroALetras(/ROOT/PAGOS/TOTAL)" />
            <fo:block font-style="italic" font-weight="bold" color="black" text-align="left">
              <xsl:value-of select="$EnLetras" />
            </fo:block>
          </fo:block-container>

          <!-- boleta 2 -->
          <fo:block-container position="absolute" top="3.5cm" left="17.5cm" height="14pt" width="100%">
            <fo:block font-style="italic" color="black">
              <xsl:value-of select="/ROOT/SUSCRIPTOR"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="4cm" left="17.5cm" height="14pt" width="100%">
            <fo:block font-style="italic" color="black">
              <xsl:value-of select="/ROOT/DOMICILIO"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="4.5cm" left="19.6cm" height="14pt" width="100%">
            <fo:block font-style="italic" color="black">
              <xsl:value-of select="/ROOT/TERMINO"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="5cm" left="17.5cm" height="14pt" width="3.3cm">
            <fo:block font-style="italic" color="black">
              &#160;<xsl:value-of select="/ROOT/FECHADESDE/DIA"/>&#160;&#160;&#160;&#160;&#160;&#160;<xsl:value-of select="/ROOT/FECHADESDE/MES"/>&#160;&#160;&#160;&#160;&#160;&#160;&#160;<xsl:value-of select="/ROOT/FECHADESDE/ANIO"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="5cm" left="22.4cm" height="14pt" width="3.3cm">
            <fo:block font-style="italic" color="black">
              &#160;<xsl:value-of select="/ROOT/FECHAHASTA/DIA"/>&#160;&#160;&#160;&#160;&#160;&#160;<xsl:value-of select="/ROOT/FECHAHASTA/MES"/>&#160;&#160;&#160;&#160;&#160;&#160;&#160;<xsl:value-of select="/ROOT/FECHAHASTA/ANIO"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="6.4cm" left="15.6cm" height="1cm" width="10.1cm">
            <fo:block font-size="11pt" font-style="italic" text-align="center" color="black">
              <xsl:value-of select="/ROOT/DIRECCIONAREMITIR/LINEA1"/>
            </fo:block>
            <fo:block font-size="11pt" font-style="italic" text-align="center" color="black">
              <xsl:value-of select="/ROOT/DIRECCIONAREMITIR/LINEA2"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="8.3cm" left="18.4cm" height="14pt" width="100%">
            <fo:block font-style="italic" color="black">
              <xsl:value-of select="/ROOT/SUCURSAL"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="8.3cm" left="24.8cm" height="14pt" width="100%">
            <fo:block font-style="italic" color="black">
              <xsl:value-of select="/ROOT/AGENCIA"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="8.95cm" left="23.4cm" height="14pt" width="1.9cm">
            <fo:block font-style="italic" color="black" text-align="right">
              <xsl:value-of select="/ROOT/PAGOS/EFECTIVO"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="9.55cm" left="23.4cm" height="14pt" width="1.9cm">
            <fo:block font-style="italic" color="black" text-align="right">
              <xsl:value-of select="/ROOT/PAGOS/CHEQUE"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="10.15cm" left="23.4cm" height="14pt" width="1.9cm">
            <fo:block font-style="italic" color="black" text-align="right">
              <xsl:value-of select="/ROOT/PAGOS/COMISION"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="10.75cm" left="23.4cm" height="14pt" width="1.9cm">
            <fo:block font-style="italic" font-weight="bold" color="black" text-align="right">
              <xsl:value-of select="/ROOT/PAGOS/TOTAL"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="12.45cm" left="16.6cm" height="16pt" width="23mm">
            <fo:block font-style="italic" color="black">
              <xsl:value-of select="/ROOT/FECHAEMISION/DIA"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="12.45cm" left="19.3cm" height="16pt" width="40mm">
            <fo:block font-style="italic" color="black">
              <xsl:value-of select="/ROOT/FECHAEMISION/MES"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="12.45cm" left="24.5cm" height="16pt" width="1.6cm">
            <fo:block font-style="italic" color="black">
              <xsl:value-of select="/ROOT/FECHAEMISION/ANIO"/>
            </fo:block>
          </fo:block-container>
          <fo:block-container position="absolute" top="11.4cm" left="17.8cm" height="18mm" width="8cm" line-height="150%">
            <xsl:variable name="EnLetras" select="nbsf:NumeroALetras(/ROOT/PAGOS/TOTAL)" />
            <fo:block font-style="italic" font-weight="bold" color="black" text-align="left">
              <xsl:value-of select="$EnLetras" />
            </fo:block>
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

            <fo:table-column column-width="4cm"/>

            <fo:table-column column-width="2mm"/>
            <fo:table-column column-width="2.5cm"/>
            <fo:table-column column-width="4.6cm"/>
            <fo:table-column column-width="1cm"/>
            <fo:table-column column-width="2cm"/>
            <fo:table-column column-width="5mm"/>
            <fo:table-column column-width="2mm"/>
            <fo:table-column column-width="proportional-column-width(1)"/>
            <fo:table-body>
              <!-- row -->
              <fo:table-row>
                <!-- boleta 1 -->
                <fo:table-cell column-number="2">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block text-align="left">
                    <fo:inline font-family="Basset" font-style="normal" font-weight="bold" font-size="12pt">SERIE A </fo:inline>
                    <fo:external-graphic src="{$ResourcePath}\escudo_gris.bmp" height="1.56cm" width="1.25cm"/>
                  </fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="3">
                  <fo:block text-align="right" margin-right="0mm">
                    <fo:inline font-size="14pt" font-family="Typist" font-style="normal" font-weight="normal">
                      N&#0176; <xsl:value-of select="/ROOT/NUMERO" />
                    </fo:inline>
                  </fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>

                <!-- boleta 2 -->
                <fo:table-cell column-number="10">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block text-align="left">
                    <fo:inline font-family="Basset" font-style="normal" font-weight="bold" font-size="12pt">SERIE A </fo:inline>
                    <fo:external-graphic src="{$ResourcePath}\escudo_gris.bmp" height="1.56cm" width="1.25cm"/>
                  </fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="3">
                  <fo:block text-align="right" margin-right="0mm">
                    <fo:inline font-size="14pt" font-family="Typist" font-style="normal" font-weight="normal">
                      N&#0176; <xsl:value-of select="/ROOT/NUMERO" />
                    </fo:inline>
                  </fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
              </fo:table-row>

              <!-- row -->

              <fo:table-row>
                <fo:table-cell column-number="2">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="5">
                  <fo:block text-align="center" space-before="4mm">
                    <fo:block font-size="18pt" font-family="Times,Helvetica" font-style="italic" font-weight="bold">BOLETIN OFICIAL</fo:block>
                    <fo:block font-size="9pt" font-style="italic" font-weight="normal">SUSCRIPCIONES</fo:block>
                  </fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>

                <fo:table-cell column-number="10">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="5">
                  <fo:block text-align="center" space-before="4mm">
                    <fo:block font-size="18pt" font-family="Times,Helvetica" font-style="italic" font-weight="bold">BOLETIN OFICIAL</fo:block>
                    <fo:block font-size="9pt" font-style="italic" font-weight="normal">SUSCRIPCIONES</fo:block>
                  </fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
              </fo:table-row>

              <!-- row -->

              <fo:table-row>
                <fo:table-cell column-number="2">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="6">
                  <fo:block text-align="left" space-before="4mm" line-height="150%" margin-left="2mm">
                    <fo:block>Suscriptor........................................................................................</fo:block>
                    <fo:block>Domicilio.........................................................................................</fo:block>
                    <fo:block>Término de suscripción...................................................................</fo:block>
                    <fo:block>Desde el ........../.........../............  hasta el .........../.........../...............</fo:block>
                  </fo:block>
                </fo:table-cell>

                <fo:table-cell column-number="10">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="6">
                  <fo:block text-align="left" space-before="4mm" line-height="150%" margin-left="2mm">
                    <fo:block>Suscriptor........................................................................................</fo:block>
                    <fo:block>Domicilio.........................................................................................</fo:block>
                    <fo:block>Término de suscripción...................................................................</fo:block>
                    <fo:block>Desde el ........../.........../............  hasta el .........../.........../...............</fo:block>
                  </fo:block>
                </fo:table-cell>
              </fo:table-row>

              <!-- row -->

              <fo:table-row>
                <fo:table-cell column-number="2">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="5">
                  <fo:block text-align="center" space-before="2mm" space-after="2mm">DIRECCION A REMITIR EL BOLETIN</fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>

                <fo:table-cell column-number="10">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="5">
                  <fo:block text-align="center" space-before="2mm" space-after="2mm">DIRECCION A REMITIR EL BOLETIN</fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
              </fo:table-row>

              <!-- row -->

              <fo:table-row>
                <fo:table-cell column-number="2">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="5" border="1pt solid black" height="1cm">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>

                <fo:table-cell column-number="10">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="5" border="1pt solid black" height="1cm">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
              </fo:table-row>

              <!-- row -->

              <fo:table-row>
                <fo:table-cell number-columns-spanned="7" column-number="2">
                  <fo:block text-align="center" space-before="2mm" font-weight="bold" font-style="italic" font-size="14pt">Nuevo Banco de Santa Fe S. A.</fo:block>
                </fo:table-cell>

                <fo:table-cell number-columns-spanned="7" column-number="10">
                  <fo:block text-align="center" space-before="2mm" font-weight="bold" font-style="italic" font-size="14pt">Nuevo Banco de Santa Fe S. A.</fo:block>
                </fo:table-cell>
              </fo:table-row>

              <!-- row -->

              <fo:table-row>
                <fo:table-cell column-number="2">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="2">
                  <fo:block text-align="left" space-before="2mm" margin-left="2mm" line-height="150%">
                    <fo:block>
                      Sucursal o casa <fo:leader leader-pattern="dots" leader-length="4.1cm"/>
                    </fo:block>
                  </fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="4">
                  <fo:block text-align="right" space-before="2mm" margin-right="2mm" line-height="150%">
                    <fo:block>
                      Agencia N° <fo:leader leader-pattern="dots" leader-length="1.5cm"/>
                    </fo:block>
                  </fo:block>
                </fo:table-cell>

                <fo:table-cell column-number="10">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="2">
                  <fo:block text-align="left" space-before="2mm" margin-left="2mm" line-height="150%">
                    <fo:block>
                      Sucursal o casa <fo:leader leader-pattern="dots" leader-length="4.1cm"/>
                    </fo:block>
                  </fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="4">
                  <fo:block text-align="right" space-before="2mm" margin-right="2mm" line-height="150%">
                    <fo:block>
                      Agencia N° <fo:leader leader-pattern="dots" leader-length="1.5cm"/>
                    </fo:block>
                  </fo:block>
                </fo:table-cell>
              </fo:table-row>

              <!-- row -->

              <fo:table-row>
                <fo:table-cell column-number="2">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="3" border-left="1pt solid black" border-top="1pt solid black" padding-bottom="0mm" padding-top="1mm" height="5mm">
                  <fo:block text-align="left" margin-left="2mm">
                    Depósito en efectivo <fo:leader leader-pattern="dots" leader-length="4.5cm"/>
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

                <fo:table-cell column-number="10">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="3" border-left="1pt solid black" border-top="1pt solid black" padding-bottom="0mm" padding-top="1mm" height="5mm">
                  <fo:block text-align="left" margin-left="2mm">
                    Depósito en efectivo <fo:leader leader-pattern="dots" leader-length="4.5cm"/>
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

              <!-- row -->

              <fo:table-row>
                <fo:table-cell column-number="2">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="3" border-left="1pt solid black" padding-bottom="0mm" padding-top="1mm" height="5mm">
                  <fo:block text-align="left" margin-left="2mm">
                    Cheque Giro N° <fo:leader leader-pattern="dots" leader-length="5.1cm"/>
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

                <fo:table-cell column-number="10">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="3" border-left="1pt solid black" padding-bottom="0mm" padding-top="1mm" height="5mm">
                  <fo:block text-align="left" margin-left="2mm">
                    Cheque Giro N° <fo:leader leader-pattern="dots" leader-length="5.1cm"/>
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

              <!-- row -->

              <fo:table-row>
                <fo:table-cell column-number="2">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="3" border-left="1pt solid black" padding-bottom="0mm" padding-top="1mm" height="5mm">
                  <fo:block text-align="left" margin-left="2mm">
                    c/Banco <fo:leader leader-pattern="dots" leader-length="6.3cm"/>
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

                <fo:table-cell column-number="10">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="3" border-left="1pt solid black" padding-bottom="0mm" padding-top="1mm" height="5mm">
                  <fo:block text-align="left" margin-left="2mm">
                    c/Banco <fo:leader leader-pattern="dots" leader-length="6.3cm"/>
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

              <!-- row -->

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

                <fo:table-cell column-number="10">
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

              <!-- row -->

              <fo:table-row>
                <fo:table-cell column-number="2">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="5">
                  <fo:block text-align="left" line-height="150%" space-before="2mm" margin-left="2mm">Son pesos.........................................................................................</fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>

                <fo:table-cell column-number="10">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="5">
                  <fo:block text-align="left" line-height="150%" space-before="2mm" margin-left="2mm">Son pesos.........................................................................................</fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
              </fo:table-row>

              <!-- row -->

              <fo:table-row>
                <fo:table-cell column-number="2">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="5">
                  <fo:block text-align="left" line-height="150%" margin-left="2mm">
                    <fo:leader leader-pattern="dots" leader-length="10.5cm"/>
                  </fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>

                <fo:table-cell column-number="10">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="5">
                  <fo:block text-align="left" line-height="150%" margin-left="2mm">
                    <fo:leader leader-pattern="dots" leader-length="10.5cm"/>
                  </fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
              </fo:table-row>

              <!-- row -->

              <fo:table-row>
                <fo:table-cell column-number="2">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="5">
                  <fo:block text-align="left" line-height="150%" margin-left="2mm">.........................de.....................................................de ...................</fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>

                <fo:table-cell column-number="10">
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="5">
                  <fo:block text-align="left" line-height="150%" margin-left="2mm">.........................de.....................................................de ...................</fo:block>
                </fo:table-cell>
                <fo:table-cell>
                  <fo:block>&#160;</fo:block>
                </fo:table-cell>
              </fo:table-row>

              <!-- row -->

              <fo:table-row>
                <fo:table-cell number-columns-spanned="7" column-number="2">
                  <fo:block text-align="center" font-size="36pt" font-family="IntHrP72DlTt">
                    <xsl:value-of select="nbsf:ConvertTo2of5(/ROOT/CODIGOBARRA)" />
                  </fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="7" column-number="10">
                  <fo:block text-align="center" font-size="36pt" font-family="IntHrP72DlTt">
                    <xsl:value-of select="nbsf:ConvertTo2of5(/ROOT/CODIGOBARRA)" />
                  </fo:block>
                </fo:table-cell>
              </fo:table-row>

              <!-- row -->

              <fo:table-row>
                <fo:table-cell number-columns-spanned="7" column-number="2">
                  <fo:block text-align="center" width="100%" font-size="9pt">Para el API.</fo:block>
                </fo:table-cell>
                <fo:table-cell number-columns-spanned="7" column-number="10">
                  <fo:block text-align="center" width="100%" font-size="9pt">Para el banco.</fo:block>
                </fo:table-cell>
              </fo:table-row>
            </fo:table-body>
          </fo:table>
          <!-- FIN DE LAS BOLETAS -->
        </fo:flow>
      </fo:page-sequence>
    </fo:root>
  </xsl:template>

</stylesheet>
