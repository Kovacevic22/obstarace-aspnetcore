import { createContext, useState, useEffect, type ReactNode } from "react";
import type { UserDto } from "../Models/users.type.ts";
import { authService } from "../services/authService.ts";

interface AuthContextType {
    user: UserDto | null;
    loading: boolean;
    setUser: (user: UserDto | null) => void;
    logout: () => Promise<void>;
    refreshUser: () => Promise<void>;
}

// eslint-disable-next-line react-refresh/only-export-components
export const AuthContext = createContext<AuthContextType | undefined>(undefined);

interface AuthProviderProps {
    children: ReactNode;
}

export const AuthProvider = ({ children }: AuthProviderProps) => {
    const [user, setUser] = useState<UserDto | null>(null);
    const [loading, setLoading] = useState(true);

    const fetchUser = async () => {
        try {
            const response = await authService.me();
            setUser(response);
        } catch (err) {
            console.log("Not authenticated:", err);
            setUser(null);
        } finally {
            setLoading(false);
        }
    };

    const logout = async () => {
        await authService.logout();
        setUser(null);
    };

    useEffect(() => {
        void fetchUser();
    }, []);

    return (
        <AuthContext.Provider value={{ user, loading, setUser, logout, refreshUser: fetchUser }}>
            {children}
        </AuthContext.Provider>
    );
};