import { Routes, Route, Navigate } from "react-router";
import HomePage from "../pages/HomePage.tsx";
import LoginPage from "../pages/auth/LoginPage.tsx";
import RegisterPage from "../pages/auth/RegisterPage.tsx";
import RacesPage from "../pages/RacesPage.tsx";
import RaceDetailsPage from "../pages/RaceDetailsPage.tsx";
import AdminDashboard from "../pages/admin/AdminDashboard.tsx";
import OrganiserDashboard from "../pages/organiser/OrganiserDashboard.tsx";
import MyRegistrationsPage from "../pages/user/MyRegistrationsPage.tsx";
import ProfilePage from "../pages/user/ProfilePage.tsx";
import OrganiserPortal from "../pages/organiser/OrganiserPortal.tsx";
import OfflinePage from "../pages/OfflinePage.tsx";
import { useAuth } from "../hooks/useAuth.ts";
import {Role} from "../Models/users.type.ts";

export const AppRoutes = () => {
    const { user } = useAuth();

    const getHomePage = () => {
        if (user?.role === Role.Admin) return <AdminDashboard />;
        if (user?.role === Role.Organiser) return <OrganiserDashboard />;
        return <HomePage />;
    };

    return (
        <Routes>
            <Route path="/" element={getHomePage()} />
            <Route path="/login" element={<LoginPage />} />
            <Route path="/register" element={<RegisterPage />} />
            <Route path="/races" element={<RacesPage />} />
            <Route path="/races/:slug" element={<RaceDetailsPage/>} />
            <Route path="/my-registrations" element={<MyRegistrationsPage/>} />
            <Route path="/profile" element={<ProfilePage/>} />
            <Route path="/organiser-portal" element={<OrganiserPortal />} />
            <Route path="/offline" element={<OfflinePage />} />
            <Route path="*" element={<Navigate to="/" replace />} />
        </Routes>
    );
};