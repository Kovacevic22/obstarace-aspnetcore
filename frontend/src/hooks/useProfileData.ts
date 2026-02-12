import { useState, useCallback } from "react";
import userService from "../services/userService.ts";
import { AxiosError } from "axios";
import type {UpdateParticipantDto} from "../Models/participant.type.ts";
import type {UserDto} from "../Models/users.type.ts";

export const useProfileData = (user: UserDto | null) => {
    const [loading, setLoading] = useState<boolean>(false);
    const [error, setError] = useState<string | null>(null);

    const [form, setForm] = useState<UpdateParticipantDto>({
        dateOfBirth: user?.participant?.dateOfBirth
            ? new Date(user.participant.dateOfBirth).toISOString().split('T')[0]
            : "",
        emergencyContact: user?.participant?.emergencyContact || "",
    });

    const parseApiError = useCallback((err: unknown): string => {
        if (err instanceof AxiosError) {
            const data = err.response?.data;
            if (data?.errors) return Object.values(data.errors).flat().join(" | ");
            return data?.title || data?.error || "Error";
        }
        return err instanceof Error ? err.message : "Unknown error";
    }, []);

    const updateProfile = async () => {
        if (!user?.id) return false;
        try {
            setLoading(true);
            setError(null);
            await userService.updateUser(user.id, form);
            return true;
        } catch (err) {
            setError(parseApiError(err));
            return false;
        } finally {
            setLoading(false);
        }
    };

    const isFormValid = form.emergencyContact.trim() !== "";

    return {
        form,
        setForm,
        loading,
        error,
        setError,
        isFormValid,
        updateProfile
    };
};