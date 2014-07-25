<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="text" />

	<xsl:template match="records">
		<xsl:apply-templates select="record" />
	</xsl:template>

	<xsl:template match="record">
    <SPAN STYLE="display: 'block'; font-family: 'arial'; color: '#008000'; font-weight: '600'; font-size: '22'; margin-top: '12pt'; text-align: 'center'">
      <xsl:for-each select="*">
        <xsl:text>"</xsl:text>
        <xsl:value-of select="." />
        <xsl:text>"</xsl:text>

        <xsl:if test="position() != last()">
          <xsl:value-of select="','" />
        </xsl:if>
      </xsl:for-each>
      <xsl:text>&#10;</xsl:text>
    </SPAN>
	</xsl:template>
   
</xsl:stylesheet>
