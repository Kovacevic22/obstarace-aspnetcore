import PublicNavbar from "../navbar/PublicNavbar.tsx";
import AdminNavbar from "../navbar/AdminNavbar.tsx";
import OrganiserNavbar from "../navbar/OrganiserNavbar.tsx";
import UserNavbar from "../navbar/UserNavbar.tsx";
import {useAuth} from "../../hooks/useAuth.ts";
import {Role} from "../../Models/users.type.ts";

export const NavbarSelector = () => {
    const { user } = useAuth();

    if (!user) return <PublicNavbar />;

    switch (user.role) {
        case Role.Admin:
            return <AdminNavbar />;
        case Role.Organiser:
            return <OrganiserNavbar />;
        default:
            return <UserNavbar />;
    }
};