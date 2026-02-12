import { useState, useEffect, useCallback } from "react";
import { registrationService } from "../services/registrationService.ts";
import { AxiosError } from "axios";
import type {RegistrationDto} from "../Models/registrations.type.ts";

export const useMyRegistrations = (userId: number | undefined) => {
    const [registrations, setRegistrations] = useState<RegistrationDto[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string | null>(null);

    const parseApiError = useCallback((err: unknown): string => {
        if (err instanceof AxiosError) {
            const data = err.response?.data;
            if (data?.errors) return Object.values(data.errors).flat().join(" | ");
            return data?.title || data?.error || "Error";
        }
        return err instanceof Error ? err.message : "Unknown error";
    }, []);

    const fetchRegistrations = useCallback(async () => {
        if (!userId) return;
        try {
            setLoading(true);
            const data = await registrationService.registrations();
            setRegistrations(data);
            setError(null);
        } catch (err) {
            setError(parseApiError(err));
        } finally {
            setLoading(false);
        }
    }, [userId, parseApiError]);

    const deleteRegistration = async (registrationId: number) => {
        try {
            await registrationService.deleteRegistration(registrationId);
            setRegistrations(prev => prev.filter(r => r.id !== registrationId));
            setError(null);
            return true;
        } catch (err) {
            setError(parseApiError(err));
            return false;
        }
    };

    useEffect(() => {
        void fetchRegistrations();
    }, [fetchRegistrations]);

    return {
        registrations,
        loading,
        error,
        deleteRegistration,
        setError
    };
};