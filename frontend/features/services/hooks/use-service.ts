import {useQueryClient, useMutation, useQuery} from '@tanstack/react-query';
import {serviceService} from '../api/service.service';
import type {CreateServiceDto, UpdateServiceDto} from '../types/service.types';
import {toast} from 'sonner';

const QUERY_KEY = ['services'] as const;

export const useServices = (page = 1, pageSize = 10, search = '') => {
  return useQuery({
    queryKey: [...QUERY_KEY, page, pageSize, search],
    queryFn: () => serviceService.getAll(page, pageSize, search),
  });
}

export const useService = (id: number) => {
  return useQuery({
    queryKey: [...QUERY_KEY, 'detail', id],
    queryFn: () => serviceService.getById(id),
    enabled: !!id,
  });
}

export const useCreateService = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (dto: CreateServiceDto) => serviceService.create(dto),
    onSuccess: () => {
        queryClient.invalidateQueries({queryKey: QUERY_KEY});
        toast.success("Servicio creado correctamente");
    },
    onError: () => {
        toast.error("No se pudo crear el servicio");
    },
});
}

export const useUpdateService = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (dto: UpdateServiceDto) => serviceService.update(dto),
    onSuccess: () => {
        queryClient.invalidateQueries({queryKey: QUERY_KEY});
        toast.success("Servicio actualizado correctamente");
    },
    onError: () => {
        toast.error("No se pudo actualizar el servicio");
    },
});
}

export const useDeleteService = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: (id: number) => serviceService.delete(id),
    onSuccess: () => {
        queryClient.invalidateQueries({queryKey: QUERY_KEY});
        toast.success("Servicio eliminado correctamente");
    },
    onError: () => {
        toast.error("No se pudo eliminar el servicio");
    },
})
}
