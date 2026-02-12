import { useState, useEffect, useCallback } from "react";
import { registrationService } from "../services/registrationService.ts";
import type {RegistrationDto} from "../Models/registrations.type.ts";
import {parseApiError} from "../utils/errorParser.ts";

export const useMyRegistrations = (userId: number | undefined) => {
    const [registrations, setRegistrations] = useState<RegistrationDto[]>([]);
    const [loading, setLoading] = useState<boolean>(true);
    const [error, setError] = useState<string|null>(null);

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
    }, [userId]);

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