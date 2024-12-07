Imports System.Data.SqlClient
Imports System.IO
Imports System.Net.Mime.MediaTypeNames
Imports System.Reflection
Imports System.Text
Imports System.Windows.Forms.VisualStyles.VisualStyleElement.Tab
Imports DocumentFormat.OpenXml.Packaging
Imports DocumentFormat.OpenXml.Wordprocessing
Imports Org.BouncyCastle.Asn1

Imports iText.Kernel.Pdf
Imports iText.Layout
Imports iText.Layout.Element
Imports DocumentFormat.OpenXml.Spreadsheet
Imports ClosedXML.Excel

Public Class ReportHomeControl
    Dim visitors As DataTable = GetTableData("visitors")
    Dim pdl As DataTable = GetTableData("pdl")
    Dim staffdetails As DataTable = GetTableData("staffdetails")
    Dim medical As DataTable = GetTableData("medical")
    Dim crime_case As DataTable = GetTableData("criminal_case")

    Private Sub cmbReports_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbReports.SelectedIndexChanged
        ' Get the selected item from the combo box
        Dim selectedReport As String = cmbReports.SelectedItem.ToString()

        ' Call the generateExcel method based on the selected item
        Select Case selectedReport.ToLower()
            Case "pdl"
                generateExcel(pdl)
            Case "visitors"
                generateExcel(visitors)
            Case "medical"
                generateExcel(medical)
            Case "crime case"
                generateExcel(crime_case)
            Case "staff"
                generateExcel(staffdetails)
            Case Else
                MessageBox.Show("Please select a valid report type.")
        End Select
    End Sub

    ' Modified generateExcel method to take the selected DataTable
    Private Sub generateExcel(table As DataTable)
        ' Create a new Excel workbook
        Using workbook As New XLWorkbook()

            ' Add the DataTable as a new sheet to the workbook
            AddDataTableToSheet(workbook, table.TableName, table)

            ' Define the file path where the Excel file will be saved
            Dim filePath As String = "C:\Path\To\Save\" & table.TableName & "_Report.xlsx"

            ' Save the workbook to the file
            workbook.SaveAs(filePath)

            ' Optionally, display a message when done
            MessageBox.Show("Excel file generated successfully at: " & filePath)
        End Using
    End Sub
    Private Sub AddDataTableToSheet(workbook As XLWorkbook, sheetName As String, table As DataTable)
        ' Add a new sheet with the given name
        Dim worksheet = workbook.Worksheets.Add(sheetName)

        ' Add column headers
        For colIndex As Integer = 0 To table.Columns.Count - 1
            worksheet.Cell(1, colIndex + 1).Value = table.Columns(colIndex).ColumnName
        Next

        ' Add rows from the DataTable
        For rowIndex As Integer = 0 To table.Rows.Count - 1
            For colIndex As Integer = 0 To table.Columns.Count - 1
                ' Get the value from the DataTable
                Dim cellValue As Object = table.Rows(rowIndex)(colIndex)

                ' Handle DBNull and convert to string
                If IsDBNull(cellValue) Then
                    worksheet.Cell(rowIndex + 2, colIndex + 1).Value = ""
                Else
                    ' Convert all values to string
                    worksheet.Cell(rowIndex + 2, colIndex + 1).Value = cellValue.ToString()
                End If
            Next
        Next
    End Sub


End Class

