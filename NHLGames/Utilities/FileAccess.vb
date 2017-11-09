﻿Imports System.IO
Imports NHLGames.My.Resources

Namespace Utilities

    Public Class FileAccess

        Public Shared Function IsFileReadonly(path As String) As Boolean
            Dim attributes As FileAttributes = File.GetAttributes(path)
            Return (attributes And FileAttributes.[ReadOnly]) = FileAttributes.[ReadOnly]
        End Function

        Public Shared Sub RemoveReadOnly(path As String)
            Dim attributes As FileAttributes = File.GetAttributes(path)

            If (attributes And FileAttributes.[ReadOnly]) = FileAttributes.[ReadOnly] Then
                ' Make the file RW
                attributes = RemoveAttribute(attributes, FileAttributes.[ReadOnly])
                File.SetAttributes(path, attributes)
                Console.WriteLine(English.msgRemoveReadOnly, path)
            End If
        End Sub

        Public Shared Sub AddReadonly(path As String)
            File.SetAttributes(path, File.GetAttributes(path) Or FileAttributes.ReadOnly)
            Console.WriteLine(English.msgAddReadOnly, path)
        End Sub

        Public Shared Function RemoveAttribute(attributes As FileAttributes, attributesToRemove As FileAttributes) As FileAttributes
            Return attributes And Not attributesToRemove
        End Function

        Public Shared Function HasAccess(filePath As String, Optional createIt As Boolean = true, Optional reportException As Boolean = false)
            Try
                If createIt Then File.WriteAllText(filePath, English.msgTestTxtContent)

                Using inputstreamreader As New StreamReader(filePath)
                    inputstreamreader.Close()
                End Using
                Using inputStream As FileStream = File.Open(filePath, FileMode.Open, IO.FileAccess.ReadWrite, FileShare.None)
                    inputStream.Close()
                End Using

                If createIt Then File.Delete(filePath)
                Return True
            Catch ex As Exception
                If reportException Then 
                    Console.WriteLine(String.Format(English.errorGeneral, "checking access to a path", ex.Message))
                End If
                Return False
            End Try
        End Function

    End Class
End Namespace
