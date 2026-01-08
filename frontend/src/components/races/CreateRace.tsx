import {useEffect, useState} from "react";
import type {CreateRaceDto} from "../../Models/races.type.ts";
import {type CreateObstacleDto, Difficulty, type ObstacleDto} from "../../Models/obstacles.type.ts";
import obstacleService from "../../services/obstacleService.ts";
import * as React from "react";
import raceService from "../../services/raceService.ts";
import ConfirmModal from "../common/ConfirmModal.tsx";

export function CreateRace({ isOpen, onClose }: { isOpen: boolean, onClose: () => void }) {
    const [raceForm, setRaceForm] = useState<CreateRaceDto>({
        name: '',
        slug: '',
        date: new Date().toISOString().slice(0, 16),
        registrationDeadLine: new Date().toISOString().slice(0, 16),
        description: '',
        location: '',
        distance: 0,
        difficulty: 0,
        status: 0,
        imageUrl: '',
        elevationGain: 0,
        maxParticipants: 100,
        obstacleIds: []
    });
    const [newObstacle, setNewObstacle] = useState<CreateObstacleDto>({
        name: '',
        description: '',
        difficulty: 0
    });
    const [oldObstacle, setOldObstacle] = useState<ObstacleDto[]>([]);
    const [isLoading, setIsLoading] = useState<boolean>(true);
    const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false);
    const [itemToDelete, setItemToDelete] = useState<number | null>(null);
    const [error, setError] = useState<string | null>(null);
    const [searchTerm, setSearchTerm] = useState<string>("");
    const isFormValid =
        raceForm.name.trim() !== "" &&
        raceForm.location.trim() !== "" &&
        raceForm.description.trim() !== "" &&
        raceForm.distance > 0 &&
        raceForm.elevationGain > 0 &&
        raceForm.imageUrl.trim() !== "" &&
        raceForm.maxParticipants>0;
    const isCreateObstacleValid =
        newObstacle.name.trim() !==""&&
        newObstacle.description.trim()!=="";
    const slugGenerator = (e: React.ChangeEvent<HTMLInputElement>) => {
        const name = e.target.value;
        const slug = name.toLowerCase().replace(/[^a-z0-9]+/g, '-').replace(/(^-|-$)/g, '');
        setRaceForm(prev => ({ ...prev, name, slug }));
    };

    const checkedObstacles = (id:number) => {
        setRaceForm(prev =>
            (
                {...prev, obstacleIds: prev.obstacleIds.includes(id)
                        ?prev.obstacleIds.filter(oid=>oid !== id)
                        :[...prev.obstacleIds, id]}
            ))
    }
    const parseApiError = (error : any): string => {
        const data = error?.response?.data;

        if (data?.errors) {
            return Object.values(data.errors).flat().join(" | ");
        }

        return data?.title || data?.error || "Error";
    };
    const createRace = async (e?:React.MouseEvent)=>{
        if(e) {
            e.preventDefault();
            e.stopPropagation();
        }
        if (!isFormValid) return;
        try{
            setIsLoading(true);
            await raceService.createRace(raceForm);
            onClose();
        }catch(error){
            console.error(error);
            setError(parseApiError(error));
        }finally {
            setIsLoading(false);
        }
    };

    const createObstacle = async (e?: React.MouseEvent)=>{
        if(e) {
            e.preventDefault();
            e.stopPropagation();
        }
        try{
            setIsLoading(true);
            const created = await obstacleService.createObstacle(newObstacle);
            setOldObstacle(prev => [...prev, created]);
            setRaceForm(prev => ({ ...prev, obstacleIds: [...prev.obstacleIds, created.id] }));
            setNewObstacle({ name: '', description: '', difficulty: 0 });
        }catch(error){
            console.error(error);
            setError(parseApiError(error));
        }finally {
            setIsLoading(false);
        }
    };
    const deleteObstacle = async () => {
        if (itemToDelete) {
            try {
                await obstacleService.removeObstacle(itemToDelete);
                setOldObstacle(prev => prev.filter(o => o.id !== itemToDelete));
                setRaceForm(prev => ({...prev, obstacleIds: prev.obstacleIds.filter(oid => oid !== itemToDelete)}));
                setItemToDelete(null);
                setIsDeleteModalOpen(false);
                setError(null);
            } catch (error) {
                console.error(error);
                setError(parseApiError(error));
            }
        }
    };
    const fetchObstacles = async (query: string) => {
        try {
            setIsLoading(true);
            const data = await obstacleService.getObstacles(query);
            setOldObstacle(data);
        } catch (error) {
            console.error(error);
            setError(parseApiError(error));
        } finally {
            setIsLoading(false);
        }
    };
    useEffect(() => {
        if (!isOpen) return;
        const delayFetch = setTimeout(() => {
            void fetchObstacles(searchTerm);
        }, 500);
        return () => clearTimeout(delayFetch);
    }, [searchTerm, isOpen]);

    if (!isOpen) return null;

    return (
        <div className="fixed inset-0 z-50 flex items-center justify-center p-0 md:p-6 font-black uppercase italic">
            <div className="absolute inset-0 bg-black/95 backdrop-blur-xl" onClick={onClose} />

            <div className="overflow-anchor-none relative w-full max-w-6xl h-full md:h-auto md:max-h-[95vh] overflow-y-auto bg-[#1a2421] border border-white/5 shadow-2xl no-scrollbar text-white">

                <div className="sticky top-0 z-10 bg-[#1a2421]/90 backdrop-blur-md p-6 md:p-8 border-b border-white/10 text-white">
                    <div className="flex justify-between items-center">
                        <div>
                            <h2 className="text-3xl md:text-4xl tracking-tighter text-accent italic leading-none">Create New Race</h2>
                            <p className="text-[10px] text-white/30 tracking-[0.3em] mt-2 not-italic font-medium uppercase text-xs">Race Configuration & Obstacle Mapping</p>
                        </div>
                        <button onClick={onClose} className="text-white/20 hover:text-white text-[10px] border border-white/10 px-4 py-2 transition-all hover:bg-white/5 cursor-pointer uppercase">
                            [ CLOSE ]
                        </button>
                    </div>
                </div>

                <div className="p-6 md:p-8 space-y-12">
                    <section>
                        <div className="grid grid-cols-1 md:grid-cols-4 gap-x-6 gap-y-8 italic font-black text-white">
                            <div className="md:col-span-3 space-y-2">
                                <label className="text-[10px] tracking-widest ml-1 uppercase flex justify-between">
                                    <span className="text-white/40">Race Name</span>
                                    {raceForm.name.trim() === "" && <span className="text-red-500 animate-pulse font-black">[ REQUIRED ]</span>}
                                </label>
                                <input
                                    onChange={slugGenerator}
                                    value={raceForm.name}
                                    className="w-full bg-white/3 border border-white/10 p-4 outline-none focus:border-accent focus:bg-white/[0.07] transition-all text-white" placeholder="Kopaonik Trail" />
                            </div>
                            <div className="space-y-2">
                                <label className="text-[10px] text-white/40 tracking-widest ml-1 uppercase text-xs">Slug</label>
                                <input
                                    value={raceForm.slug}
                                    readOnly={true}
                                    className="w-full bg-white/3 border border-white/10 p-4 outline-none text-white/50" placeholder="kopaonik-trail" />
                            </div>

                            <div className="md:col-span-2 space-y-2">
                                <label className="text-[10px]  tracking-widest ml-1 uppercase text-xs text-accent">Race Start Date & Time (AM/PM)</label>
                                <input
                                    value={raceForm.date}
                                    onChange={(e)=>setRaceForm({...raceForm, date: e.target.value})}
                                    type="datetime-local"
                                    step="60"
                                    style={{ colorScheme: 'dark' }}
                                    className="w-full bg-white/3 border border-white/10 p-5 outline-none focus:border-accent transition-all uppercase text-lg shadow-inner text-white" />
                            </div>
                            <div className="md:col-span-2 space-y-2">
                                <label className="text-[10px] tracking-widest ml-1 uppercase text-xs flex justify-between">
                                    <span className="text-white">Location</span>
                                    {raceForm.location.trim() === "" && <span className="text-red-500 animate-pulse font-black">[ REQUIRED ]</span>}
                                </label>
                                <input
                                    value={raceForm.location}
                                    onChange={(e)=>setRaceForm({...raceForm, location: e.target.value})}
                                    className="w-full bg-white/3 border border-white/10 p-5 outline-none focus:border-accent transition-all text-lg text-white" placeholder="Kopaonik, Serbia" />
                            </div>
                            <div className="space-y-2 text-white">
                                <label className="text-[10px] tracking-widest ml-1 uppercase text-xs flex justify-between">
                                    <span className="text-white/40">Distance (KM)</span>
                                    {raceForm.distance <= 0 && <span className="text-red-500 animate-pulse font-black">[ EMPTY ]</span>}
                                </label>
                                <input
                                    value={raceForm.distance}
                                    onChange={(e)=> setRaceForm({...raceForm, distance: Number(e.target.value)})}
                                    type="number" step="0.1" className="w-full bg-white/3 border border-white/10 p-4 outline-none focus:border-accent transition-all text-lg text-white" />
                            </div>

                            <div className="space-y-2 text-white">
                                <label className="text-[10px] text-white/40 tracking-widest ml-1 uppercase text-xs">Difficulty</label>
                                <select
                                    value={raceForm.difficulty}
                                    onChange={(e) => setRaceForm({...raceForm, difficulty: Number(e.target.value)})}
                                    className="w-full bg-white/3 border border-white/10 p-4 outline-none appearance-none cursor-pointer focus:border-accent transition-all uppercase text-lg text-white">
                                    <option value="0">Easy</option>
                                    <option value="1">Normal</option>
                                    <option value="2">Hard</option>
                                </select>
                            </div>
                            <div className="space-y-2 text-white">
                                <label className="text-[10px] tracking-widest ml-1 uppercase text-xs flex justify-between">
                                    <span className="text-white/40">Max Participants</span>
                                    {raceForm.maxParticipants <= 0 && <span className="text-red-500 animate-pulse font-black">[ EMPTY ]</span>}
                                </label>
                                <input
                                    value={raceForm.maxParticipants}
                                    onChange={(e)=>setRaceForm({...raceForm, maxParticipants: Number(e.target.value)})}
                                    type="number" className="w-full bg-white/3 border border-white/10 p-4 outline-none focus:border-accent transition-all text-lg text-white" />
                            </div>
                            <div className="space-y-2 text-white">
                                <label className="text-[10px] tracking-widest ml-1 uppercase text-xs flex justify-between">
                                    <span className="text-white/40">Elevation Gain (M)</span>
                                    {raceForm.elevationGain <= 0 && <span className="text-red-500 animate-pulse font-black">[ EMPTY ]</span>}
                                </label>
                                <input
                                    value={raceForm.elevationGain}
                                    onChange={(e)=>setRaceForm({...raceForm, elevationGain: Number(e.target.value)})}
                                    type="number" className="w-full bg-white/3 border border-white/10 p-4 outline-none focus:border-accent transition-all text-lg text-white" placeholder="500" />
                            </div>

                            <div className="md:col-span-2 space-y-2 text-white">
                                <label className="text-[10px]  tracking-widest ml-1 uppercase text-xs text-accent">Registration Deadline</label>
                                <input
                                    value={raceForm.registrationDeadLine}
                                    onChange={(e)=>setRaceForm({...raceForm, registrationDeadLine: e.target.value})}
                                    type="datetime-local"
                                    step="60"
                                    style={{ colorScheme: 'dark' }}
                                    className="w-full bg-white/3 border border-white/10 p-5 outline-none focus:border-accent transition-all uppercase text-lg shadow-inner text-white" />
                            </div>

                            <div className="md:col-span-2 space-y-2 text-white">
                                <label className="text-[10px] tracking-widest ml-1 uppercase text-xs flex justify-between">
                                    <span className="text-white/40">Race Banner URL</span>
                                    {raceForm.imageUrl.trim() === "" && <span className="text-red-500 animate-pulse font-black">[ REQUIRED ]</span>}
                                </label>
                                <input
                                    value={raceForm.imageUrl}
                                    onChange={(e)=>setRaceForm({...raceForm, imageUrl: e.target.value})}
                                    type="url" className="w-full bg-white/3 border border-white/10 p-5 outline-none focus:border-accent transition-all text-lg italic font-black text-white" placeholder="https://example.com/race-image.jpg" />
                            </div>
                            <div className="md:col-span-4 space-y-2 text-white">
                                <label className="text-[10px] tracking-widest ml-1 uppercase text-xs flex justify-between">
                                    <span className="text-white/40">Race Briefing (Description)</span>
                                    {raceForm.description.trim() === "" && <span className="text-red-500 animate-pulse font-black">[ REQUIRED ]</span>}
                                </label>
                                <textarea
                                    value={raceForm.description}
                                    onChange={(e) => setRaceForm({ ...raceForm, description: e.target.value })}
                                    rows={4}
                                    className="w-full bg-white/3 border border-white/10 p-4 outline-none focus:border-accent transition-all text-sm italic font-black text-white resize-none"
                                    placeholder="Enter race details, technical requirements, and briefing..."
                                />
                            </div>
                        </div>
                    </section>

                    <section className="bg-white/2 border border-white/5 p-6 rounded-sm text-white">
                        <h3 className="text-[10px] tracking-[0.4em] mb-6 text-green-500/60 font-black not-italic uppercase text-xs">Quick Add New Obstacle</h3>
                        <div className="grid grid-cols-1 md:grid-cols-4 gap-4 items-end">
                            <input
                                value={newObstacle.name}
                                onChange={(e)=>setNewObstacle({...newObstacle, name: e.target.value})}
                                placeholder="Obstacle Name" className="bg-dark border border-white/10 p-4 text-xs outline-none focus:border-green-500 transition-all uppercase font-black text-white" />
                            <input
                                value={newObstacle.description}
                                onChange={(e) => setNewObstacle({...newObstacle, description: e.target.value})}
                                placeholder="Short Description" className="bg-dark border border-white/10 p-4 text-xs outline-none focus:border-green-500 transition-all uppercase placeholder:italic font-black text-white" />
                            <select
                                value={newObstacle.difficulty}
                                onChange={(e) => setNewObstacle({...newObstacle, difficulty: Number(e.target.value)})}
                                className="bg-dark border border-white/10 p-4 text-xs outline-none cursor-pointer uppercase font-black text-white">
                                <option value="0">Easy</option>
                                <option value="1">Normal</option>
                                <option value="2">Hard</option>
                            </select>
                            <button
                                onClick={(e) => createObstacle(e)}
                                disabled={isLoading || !isCreateObstacleValid}
                                className={!isCreateObstacleValid?"opacity-50 cursor-not-allowed bg-green-600/10 text-green-500 border border-green-500/30 p-4 text-[11px] hover:bg-green-600 hover:text-white transition-all font-black uppercase shadow-lg":"bg-green-600/10 text-green-500 border border-green-500/30 p-4 text-[11px] hover:bg-green-600 hover:text-white transition-all cursor-pointer font-black uppercase shadow-lg"}>
                                {isLoading ? "DEPLOYING..." : "CREATE & ADD"}
                            </button>
                        </div>
                    </section>

                    <section className="space-y-6 pb-4 text-white uppercase">
                        <div className="flex flex-col md:flex-row justify-between items-end md:items-center gap-4">
                            <h3 className="tracking-tighter italic font-black uppercase text-3xl">Obstacle Library</h3>
                            <div className="relative w-full md:w-80">
                                <input
                                    type="text"
                                    value={searchTerm}
                                    onChange={(e) => setSearchTerm(e.target.value)}
                                    placeholder="Filter library..."
                                    className="w-full bg-white/3 border border-white/10 p-4 pl-10 text-[10px] outline-none focus:border-accent italic font-black text-white" />
                                <span className="absolute left-4 top-1/2 -translate-y-1/2 text-white/20 font-black italic tracking-tighter uppercase">🔍</span>
                            </div>
                        </div>

                        <div className="relative border border-white/10 rounded-sm overflow-hidden bg-white/1 shadow-xl">
                            {isLoading ? (
                                <div className="flex items-center justify-center p-12 bg-white/2">
                                    <div className="text-accent font-black animate-pulse tracking-[0.5em]">SEARCHING...</div>
                                </div>
                            ) : (
                                <div className="max-h-96 overflow-y-auto no-scrollbar scroll-smooth">
                                    <table className="w-full text-left text-[11px] border-collapse uppercase text-white font-black italic">
                                        <thead className="bg-white/5 border-b border-white/10 uppercase tracking-widest font-black italic text-white sticky top-0 z-10">
                                        <tr>
                                            <th className="p-6 w-16 text-center text-white">SELECT</th>
                                            <th className="p-6 text-white uppercase italic">Name</th>
                                            <th className="p-6 hidden md:table-cell text-white uppercase italic font-black">Description</th>
                                            <th className="p-6 text-center text-white italic uppercase">Difficulty</th>
                                            <th className="p-6 text-right text-white italic uppercase font-black">Action</th>
                                        </tr>
                                        </thead>
                                        <tbody className="divide-y divide-white/5 italic font-black text-sm text-white">
                                        {oldObstacle.map((obstacle) =>(
                                            <tr key={obstacle.id} className="hover:bg-white/4 transition-all group border-b border-white/5 text-white">
                                                <td className="p-6 text-center">
                                                    <input
                                                        checked={raceForm.obstacleIds.includes(obstacle.id)}
                                                        onChange={() => checkedObstacles(obstacle.id)}
                                                        type="checkbox" className="accent-accent w-6 h-6 cursor-pointer opacity-40 checked:opacity-100" />
                                                </td>
                                                <td className="p-6 font-black text-white/90 text-base">{obstacle.name}</td>
                                                <td className="p-6 text-white/40 not-italic hidden md:table-cell normal-case font-medium text-xs">{obstacle.description}</td>
                                                <td className="p-6 text-center">
                                                    <span className={`px-4 py-2 border text-[11px] font-black tracking-widest uppercase italic ${
                                                        obstacle.difficulty === 2 ? 'bg-red-500/10 text-red-500 border-red-500/20' :
                                                            obstacle.difficulty === 1 ? 'bg-orange-500/10 text-orange-500 border-orange-500/20' :
                                                                'bg-green-500/10 text-green-500 border-green-500/20'
                                                    }`}>
                                                        {Difficulty[obstacle.difficulty]}
                                                    </span>
                                                </td>
                                                <td className="p-6 text-right">
                                                    <button
                                                        onClick={() => { setItemToDelete(obstacle.id); setIsDeleteModalOpen(true); }}
                                                        className="text-red-500/40 hover:text-red-500 cursor-pointer transition-all text-[11px] font-black tracking-tighter whitespace-nowrap uppercase italic">
                                                        [ REMOVE ]
                                                    </button>
                                                </td>
                                            </tr>
                                        ))}
                                        </tbody>
                                    </table>
                                </div>
                            )}
                        </div>
                    </section>

                    <div className="min-h-15 flex items-end">
                        {error && (
                            <div className="w-full bg-red-500/10 border border-red-500/50 p-4 animate-in fade-in slide-in-from-top-1">
                                <p className="text-red-500 text-xs font-black tracking-widest uppercase">
                                    [ ERROR ]: {error}
                                </p>
                            </div>
                        )}
                    </div>

                    <div className="bg-[#1a2421] pb-8 pt-4">
                        <button
                            onClick={(e) => createRace(e)}
                            disabled={isLoading || !isFormValid}
                            className={!isFormValid ? "opacity-50 cursor-not-allowed w-full bg-accent text-dark py-6 text-3xl font-black shadow-[0_0_60px_rgba(166,124,82,0.3)] hover:bg-accent/90 hover:shadow-[0_0_80px_rgba(166,124,82,0.5)] transition-all transform active:scale-[0.98] uppercase italic tracking-tighter" : "w-full bg-accent text-dark py-6 text-3xl font-black shadow-[0_0_60px_rgba(166,124,82,0.3)] hover:bg-accent/90 hover:shadow-[0_0_80px_rgba(166,124,82,0.5)] transition-all cursor-pointer transform active:scale-[0.98] uppercase italic tracking-tighter"}>
                            {isLoading ? "DEPLOYING..." : !isFormValid ? "FORM INCOMPLETE" : "CREATE RACE"}
                        </button>
                    </div>
                </div>
            </div>

            <ConfirmModal
                isOpen={isDeleteModalOpen}
                onClose={() => setIsDeleteModalOpen(false)}
                onConfirm={deleteObstacle}
                error={error}
                title="WARNING"
                message="YOU WILL PERMANENTLY DELETE THIS OBSTACLE FROM THE SYSTEM ARCHIVE. THIS ACTION CANNOT BE REVERTED."
            />
        </div>
    );
}
export default CreateRace;