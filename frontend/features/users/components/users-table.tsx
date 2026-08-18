"use client"

import { useState } from "react"
import { useUsers, useCreateUser, useChangeRole, useDeleteUser } from "../hooks/use-users"
import { UserFormModal } from "./user-form-modal"
import { Button } from "@/components/ui/button"
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card"
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from "@/components/ui/table"
import { Badge } from "@/components/ui/badge"
import type { User, CreateUserDto, ChangeRoleDto } from "../types/user.types"

export function UsersTable() {
  const { data: users, isLoading } = useUsers()
  const createUser = useCreateUser()
  const changeRole = useChangeRole()
  const deleteUser = useDeleteUser()

  const [modalOpen, setModalOpen] = useState(false)
  const [selectedUser, setSelectedUser] = useState<User | undefined>(undefined)

  async function handleSave(dto: CreateUserDto | ChangeRoleDto, id?: string) {
    if (id) {
      await changeRole.mutateAsync({ id, dto: dto as ChangeRoleDto })
    } else {
      await createUser.mutateAsync(dto as CreateUserDto)
    }
    setModalOpen(false)
    setSelectedUser(undefined)
  }

  function handleEdit(user: User) {
    setSelectedUser(user)
    setModalOpen(true)
  }

  function handleNew() {
    setSelectedUser(undefined)
    setModalOpen(true)
  }

  async function handleDelete(id: string) {
    if (!confirm("¿Estás seguro de eliminar este usuario?")) return
    await deleteUser.mutateAsync(id)
  }

  function getRoleBadgeColor(role: string) {
    switch (role) {
      case "Admin": return "destructive"
      case "Veterinarian": return "default"
      case "Assistant": return "secondary"
      default: return "outline"
    }
  }

  if (isLoading) return <p className="p-4">Cargando usuarios...</p>

  return (
    <div className="p-6 space-y-4">
      <div className="flex justify-between items-center">
        <h1 className="text-2xl font-bold">Usuarios</h1>
        <Button onClick={handleNew} className="bg-primary hover:opacity-90 text-primary-foreground">
          + Nuevo Usuario
        </Button>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Lista de Usuarios</CardTitle>
        </CardHeader>
        <CardContent>
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>Nombre</TableHead>
                <TableHead>Email</TableHead>
                <TableHead>Rol</TableHead>
                <TableHead className="text-right">Acciones</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {users && users.length > 0 ? (
                users.map((u) => (
                  <TableRow key={u.id}>
                    <TableCell>{u.fullName}</TableCell>
                    <TableCell>{u.email}</TableCell>
                    <TableCell>
                      <Badge variant={getRoleBadgeColor(u.role)}>
                        {u.role}
                      </Badge>
                    </TableCell>
                    <TableCell className="text-right space-x-2">
                      <Button
                        variant="outline"
                        size="sm"
                        onClick={() => handleEdit(u)}
                      >
                        Cambiar Rol
                      </Button>
                      <Button
                        variant="destructive"
                        size="sm"
                        onClick={() => handleDelete(u.id)}
                        disabled={deleteUser.isPending}
                      >
                        Eliminar
                      </Button>
                    </TableCell>
                  </TableRow>
                ))
              ) : (
                <TableRow>
                  <TableCell colSpan={4} className="text-center text-muted-foreground">
                    No hay usuarios registrados
                  </TableCell>
                </TableRow>
              )}
            </TableBody>
          </Table>
        </CardContent>
      </Card>

      <UserFormModal
        open={modalOpen}
        onOpenChange={setModalOpen}
        isSaving={createUser.isPending || changeRole.isPending}
        onSave={handleSave}
        user={selectedUser}
      />
    </div>
  )
}